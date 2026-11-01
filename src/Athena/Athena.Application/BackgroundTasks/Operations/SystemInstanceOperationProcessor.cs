using Athena.Application.SystemManagement.Contracts;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Athena.Application.BackgroundTasks.Operations;

public sealed class SystemInstanceOperationProcessor(
    ISystemInstanceOperationRepository operationRepository,
    ISystemInstanceRepository instanceRepository,
    ISystemInstanceLifecycleRepository lifecycleRepository,
    ISystemInstanceExecutionAgent legacyAgent,
    ICorrelatedSystemInstanceExecutionAgent correlatedAgent,
    ILogger<SystemInstanceOperationProcessor> logger)
{
    public async Task<bool> ProcessOneAsync(
        bool useLegacyProtocol,
        TimeSpan leaseDuration,
        TimeSpan retryDelay,
        CancellationToken cancellationToken = default)
    {
        var leaseOwner = Guid.NewGuid().ToString("N");
        var nowUtc = DateTime.UtcNow;
        var operation = await operationRepository.ClaimNextAsync(leaseOwner, leaseDuration, nowUtc, cancellationToken);
        if (operation is null) return false;

        try
        {
            if (operation.AttemptCount > operation.MaxAttempts)
            {
                if (operation.Status == SystemInstanceOperationStatus.Pending
                    && operation.CommandDispatchedAtUtc is null)
                    operation.FailBeforeDispatch($"Operation exceeded its maximum of {operation.MaxAttempts} attempts before dispatch.");
                else
                {
                    if (operation.Status == SystemInstanceOperationStatus.Running)
                        operation.MarkOutcomeUnknown($"Operation exceeded its maximum of {operation.MaxAttempts} attempts; Agent outcome requires manual review.");
                    operation.MarkIndeterminate($"Operation exceeded its maximum of {operation.MaxAttempts} attempts; Agent outcome requires manual review.", leaseOwner);
                }
                if (!await operationRepository.SaveIfLeaseOwnerAsync(operation, leaseOwner, CancellationToken.None))
                    logger.LogWarning("Could not persist exhausted-operation state for {OperationId} because its lease changed.", operation.Id);
                return true;
            }
            var isNewCommand = operation.Status == SystemInstanceOperationStatus.Pending;
            var instance = await instanceRepository.GetByIdAsync(operation.SystemInstanceId, cancellationToken);
            if (isNewCommand && !CanDispatch(operation.Type, instance))
            {
                operation.FailBeforeDispatch($"SystemInstance '{operation.SystemInstanceId.Value}' is not in a valid registration state for '{operation.Type}'.");
                if (!await operationRepository.SaveIfLeaseOwnerAsync(operation, leaseOwner, CancellationToken.None))
                    logger.LogWarning("Could not persist pre-dispatch failure for operation {OperationId} because its lease changed.", operation.Id);
                return true;
            }
            if (isNewCommand && useLegacyProtocol
                && operation.Type is SystemInstanceOperationType.Configure or SystemInstanceOperationType.Unregister)
            {
                operation.FailBeforeDispatch($"The Legacy Agent protocol does not support '{operation.Type}'. Configure a correlated Agent protocol.");
                if (!await operationRepository.SaveIfLeaseOwnerAsync(operation, leaseOwner, CancellationToken.None))
                    logger.LogWarning("Could not persist unsupported legacy operation {OperationId} because its lease changed.", operation.Id);
                return true;
            }
            if (instance is null)
                throw new InvalidOperationException($"SystemInstance '{operation.SystemInstanceId.Value}' for operation '{operation.Id}' was not found.");
            var lifecycle = await lifecycleRepository.GetByIdAsync(operation.SystemInstanceId, cancellationToken)
                ?? throw new InvalidOperationException($"Lifecycle '{operation.SystemInstanceId.Value}' for operation '{operation.Id}' was not found.");

            if (isNewCommand)
            {
                operation.Start();
                operation.MarkCommandDispatched(DateTime.UtcNow);
                await SaveOwnedAsync(operation, leaseOwner, cancellationToken);
            }
            else if (operation.CommandDispatchedAtUtc is null)
            {
                if (operation.Status == SystemInstanceOperationStatus.Running)
                    operation.MarkOutcomeUnknown("Operation predates persisted Agent command correlation; automatic redelivery is unsafe.");
                operation.ScheduleRetry(DateTime.UtcNow + retryDelay);
                operation.ReleaseLease(leaseOwner);
                await SaveOwnedAsync(operation, leaseOwner, cancellationToken);
                return true;
            }

            if (useLegacyProtocol)
                await ProcessLegacyAsync(operation, instance, lifecycle, leaseOwner, isNewCommand, leaseDuration, cancellationToken);
            else
                await ProcessCorrelatedAsync(operation, instance, lifecycle, leaseOwner, isNewCommand, leaseDuration, cancellationToken);

            if (operation.IsActive)
            {
                operation.ScheduleRetry(DateTime.UtcNow + retryDelay);
                operation.ReleaseLease(leaseOwner);
                await SaveOwnedAsync(operation, leaseOwner, cancellationToken);
            }

            return true;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            await MarkUncertainAndReleaseAsync(operation, leaseOwner, "Worker shutdown interrupted processing; command outcome requires recovery.");
            throw;
        }
        catch (HttpRequestException exception) when (IsDefinitiveRejection(exception)
            && operation.Status is SystemInstanceOperationStatus.Running or SystemInstanceOperationStatus.OutcomeUnknown)
        {
            operation.Fail(exception.Message);
            if (!await operationRepository.SaveIfLeaseOwnerAsync(operation, leaseOwner, CancellationToken.None))
                logger.LogWarning("Definitive rejection for operation {OperationId} could not be saved because its lease changed.", operation.Id);
            else
                logger.LogWarning(exception, "Agent definitively rejected operation {OperationId} for {SystemInstanceId}.", operation.Id, operation.SystemInstanceId.Value);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                "Processing SystemInstance operation {OperationId} for {SystemInstanceId} failed; preserving it as uncertain.",
                operation.Id,
                operation.SystemInstanceId.Value);
            await MarkUncertainAndReleaseAsync(operation, leaseOwner, exception.Message);
        }

        return true;
    }

    private async Task ProcessCorrelatedAsync(
        SystemInstanceOperation operation,
        SystemInstance instance,
        SystemInstanceLifecycle lifecycle,
        string leaseOwner,
        bool isNewCommand,
        TimeSpan leaseDuration,
        CancellationToken cancellationToken)
    {
        await RenewLeaseOrThrowAsync(operation, leaseOwner, leaseDuration, cancellationToken);
        SystemInstanceExecutionResult result;
        if (isNewCommand)
        {
            result = await correlatedAgent.ExecuteAsync(ToCommand(operation, instance), cancellationToken);
        }
        else
        {
            result = await correlatedAgent.GetOperationAsync(
                operation.SystemInstanceId.Value,
                operation.Id,
                cancellationToken);
            if (result.Status == AgentOperationStatus.NotFound)
                result = await correlatedAgent.ExecuteAsync(ToCommand(operation, instance), cancellationToken);
        }

        ValidateResult(result, operation);
        ApplyCorrelatedResult(operation, result);
        if (isNewCommand && result.Status == AgentOperationStatus.NotFound)
            operation.MarkOutcomeUnknown("Agent did not confirm receipt of the newly dispatched OperationId.");
        SystemInstance? changedInstance = null;
        if (result.Status == AgentOperationStatus.Completed)
        {
            if (result.ObservedState is not null)
                Observe(lifecycle, result.ObservedState, result.ObservedAtUtc, operation.SystemInstanceId.Value);
            changedInstance = ApplyRegistryResult(operation.Type, instance);
        }
        if (result.Status == AgentOperationStatus.Completed && (result.ObservedState is not null || changedInstance is not null))
            await SaveOwnedWithLifecycleAsync(operation, lifecycle, leaseOwner, changedInstance, cancellationToken);
        else
            await SaveOwnedAsync(operation, leaseOwner, cancellationToken);
    }

    private async Task ProcessLegacyAsync(
        SystemInstanceOperation operation,
        SystemInstance instance,
        SystemInstanceLifecycle lifecycle,
        string leaseOwner,
        bool isNewCommand,
        TimeSpan leaseDuration,
        CancellationToken cancellationToken)
    {
        await RenewLeaseOrThrowAsync(operation, leaseOwner, leaseDuration, cancellationToken);
        SystemInstanceExecutionStatus status;
        if (isNewCommand)
        {
            status = operation.Type switch
            {
                SystemInstanceOperationType.Create => await legacyAgent.CreateAsync(ToRequest(operation, instance), cancellationToken),
                SystemInstanceOperationType.Register => await legacyAgent.RegisterAsync(ToRequest(operation, instance), cancellationToken),
                SystemInstanceOperationType.Start => await legacyAgent.StartAsync(operation.SystemInstanceId.Value, cancellationToken),
                SystemInstanceOperationType.Stop => await legacyAgent.StopAsync(operation.SystemInstanceId.Value, cancellationToken),
                SystemInstanceOperationType.Restart => await legacyAgent.RestartAsync(operation.SystemInstanceId.Value, cancellationToken),
                SystemInstanceOperationType.Remove => await legacyAgent.RemoveAsync(operation.SystemInstanceId.Value, cancellationToken),
                _ => throw new InvalidOperationException($"Unsupported operation type '{operation.Type}'.")
            };
        }
        else
        {
            status = await legacyAgent.GetStatusAsync(operation.SystemInstanceId.Value, cancellationToken);
        }

        ValidateLegacyStatus(status, operation.SystemInstanceId.Value);
        Observe(lifecycle, status.State, status.ObservedAtUtc, operation.SystemInstanceId.Value);
        if (operation.Status == SystemInstanceOperationStatus.Running)
            operation.MarkOutcomeUnknown("Legacy Agent response is not correlated with this OperationId.");
        await SaveOwnedWithLifecycleAsync(operation, lifecycle, leaseOwner, null, cancellationToken);
    }

    private async Task MarkUncertainAndReleaseAsync(
        SystemInstanceOperation operation,
        string leaseOwner,
        string reason)
    {
        try
        {
            if (operation.Status == SystemInstanceOperationStatus.Running)
                operation.MarkOutcomeUnknown(reason);
            if (operation.IsActive)
                operation.ScheduleRetry(DateTime.UtcNow.AddSeconds(5));
            operation.ReleaseLease(leaseOwner);
            if (!await operationRepository.SaveIfLeaseOwnerAsync(operation, leaseOwner, CancellationToken.None))
                logger.LogWarning("Lease for SystemInstance operation {OperationId} expired or changed before uncertainty could be persisted.", operation.Id);
        }
        catch (Exception persistenceException)
        {
            logger.LogError(persistenceException, "Could not persist uncertainty for SystemInstance operation {OperationId}.", operation.Id);
        }
    }

    private async Task SaveOwnedAsync(SystemInstanceOperation operation, string leaseOwner, CancellationToken cancellationToken)
    {
        if (!await operationRepository.SaveIfLeaseOwnerAsync(operation, leaseOwner, cancellationToken))
            throw new InvalidOperationException($"Lease ownership was lost while processing operation '{operation.Id}'.");
    }

    private async Task SaveOwnedWithLifecycleAsync(
        SystemInstanceOperation operation,
        SystemInstanceLifecycle lifecycle,
        string leaseOwner,
        SystemInstance? systemInstance,
        CancellationToken cancellationToken)
    {
        if (!await operationRepository.SaveWithLifecycleIfLeaseOwnerAsync(operation, lifecycle, systemInstance, leaseOwner, cancellationToken))
            throw new InvalidOperationException($"Lease ownership was lost while persisting operation '{operation.Id}' and its observed lifecycle.");
    }

    private async Task RenewLeaseOrThrowAsync(
        SystemInstanceOperation operation,
        string leaseOwner,
        TimeSpan leaseDuration,
        CancellationToken cancellationToken)
    {
        if (!await operationRepository.RenewLeaseAsync(
                operation.Id,
                leaseOwner,
                leaseDuration,
                DateTime.UtcNow,
                cancellationToken))
            throw new InvalidOperationException($"Lease ownership was lost before contacting the Agent for operation '{operation.Id}'.");
    }

    private static void ApplyCorrelatedResult(SystemInstanceOperation operation, SystemInstanceExecutionResult result)
    {
        if (result.AcceptedAtUtc is { } acceptedAtUtc && operation.AgentAcceptedAtUtc is null)
            operation.MarkAccepted(acceptedAtUtc);

        switch (result.Status)
        {
            case AgentOperationStatus.Accepted:
            case AgentOperationStatus.Running:
            case AgentOperationStatus.Pending:
            case AgentOperationStatus.NotFound:
                return;
            case AgentOperationStatus.OutcomeUnknown:
                if (operation.Status == SystemInstanceOperationStatus.Running)
                    operation.MarkOutcomeUnknown(result.ErrorMessage ?? "Execution Agent reports an uncertain outcome.");
                return;
            case AgentOperationStatus.Completed:
                if (result.CompletedAtUtc is not { } completedAtUtc)
                    throw new InvalidOperationException($"Agent returned Completed without CompletedAtUtc for operation '{operation.Id}'.");
                operation.Complete(completedAtUtc);
                return;
            case AgentOperationStatus.Failed:
                if (string.IsNullOrWhiteSpace(result.ErrorMessage))
                    throw new InvalidOperationException($"Agent returned Failed without an error for operation '{operation.Id}'.");
                operation.Fail(result.ErrorMessage);
                return;
            default:
                throw new InvalidOperationException($"Agent returned unsupported operation status '{result.Status}'.");
        }
    }

    private static void ValidateResult(SystemInstanceExecutionResult result, SystemInstanceOperation operation)
    {
        if (result.OperationId != operation.Id
            || result.SystemInstanceId != operation.SystemInstanceId.Value
            || result.CorrelationId is { } correlationId && correlationId != operation.Id)
            throw new InvalidOperationException($"Agent result correlation mismatch for operation '{operation.Id}'.");
        if (!Enum.IsDefined(result.Status) || result.UpdatedAtUtc.Kind != DateTimeKind.Utc)
            throw new InvalidOperationException($"Agent returned invalid execution-result metadata for operation '{operation.Id}'.");
        if (result.AcceptedAtUtc is { Kind: not DateTimeKind.Utc } || result.CompletedAtUtc is { Kind: not DateTimeKind.Utc })
            throw new InvalidOperationException($"Agent returned non-UTC execution timestamps for operation '{operation.Id}'.");
        if (result.ObservedAtUtc is { Kind: not DateTimeKind.Utc })
            throw new InvalidOperationException($"Agent returned a non-UTC observation timestamp for operation '{operation.Id}'.");
    }

    private static void ValidateLegacyStatus(SystemInstanceExecutionStatus status, Guid systemInstanceId)
    {
        if (status.SystemInstanceId != systemInstanceId
            || string.IsNullOrWhiteSpace(status.State)
            || status.ObservedAtUtc.Kind != DateTimeKind.Utc)
            throw new InvalidOperationException($"Legacy Agent returned invalid current-state data for SystemInstance '{systemInstanceId}'.");
    }

    private static void Observe(SystemInstanceLifecycle lifecycle, string state, DateTime? observedAtUtc, Guid systemInstanceId)
    {
        var mapped = state.Trim().ToLowerInvariant() switch
        {
            "running" or "active" => SystemInstanceOperationalStatus.Running,
            "stopped" or "exited" => SystemInstanceOperationalStatus.Stopped,
            "starting" => SystemInstanceOperationalStatus.Starting,
            "stopping" => SystemInstanceOperationalStatus.Stopping,
            "failed" or "error" => SystemInstanceOperationalStatus.Failed,
            "unreachable" or "unknown" => SystemInstanceOperationalStatus.Unreachable,
            _ => throw new InvalidOperationException($"Agent returned unsupported state '{state}' for SystemInstance '{systemInstanceId}'.")
        };
        if (observedAtUtc is null)
            throw new InvalidOperationException($"Agent did not provide an observation timestamp for SystemInstance '{systemInstanceId}'.");
        lifecycle.SetObservedOperationalStatus(mapped, observedAtUtc.Value);
    }

    private static SystemInstanceExecutionCommand ToCommand(SystemInstanceOperation operation, SystemInstance instance) =>
        new(
            operation.Id,
            operation.SystemInstanceId.Value,
            operation.Type.ToString(),
            operation.CommandPayloadJson,
            instance.SystemId.Value,
            instance.WorkloadId.Value,
            instance.Name.Value,
            operation.Id);

    private static SystemInstanceExecutionCommandRequest ToRequest(SystemInstanceOperation operation, SystemInstance instance) =>
        new(
            instance.Id.Value,
            instance.SystemId.Value,
            instance.WorkloadId.Value,
            instance.Name.Value,
            ConfigurationReference: GetConfigurationReference(operation.CommandPayloadJson));

    private static string? GetConfigurationReference(string? payloadJson)
    {
        if (string.IsNullOrWhiteSpace(payloadJson)) return null;
        using var document = JsonDocument.Parse(payloadJson);
        return document.RootElement.TryGetProperty("configurationReference", out var reference)
            ? reference.GetString()
            : null;
    }

    private static SystemInstance? ApplyRegistryResult(SystemInstanceOperationType type, SystemInstance instance)
    {
        switch (type)
        {
            case SystemInstanceOperationType.Register:
                instance.Register();
                return instance;
            case SystemInstanceOperationType.Unregister:
                instance.Unregister();
                return instance;
            case SystemInstanceOperationType.Remove:
                instance.Delete();
                return instance;
            default:
                return null;
        }
    }

    private static bool CanDispatch(SystemInstanceOperationType type, SystemInstance? instance)
    {
        if (instance is null || instance.IsDeleted) return false;
        return type switch
        {
            SystemInstanceOperationType.Create or SystemInstanceOperationType.Register =>
                instance.RegistrationStatus == SystemInstanceRegistrationStatus.Unregistered,
            SystemInstanceOperationType.Unregister or SystemInstanceOperationType.Remove =>
                instance.RegistrationStatus is SystemInstanceRegistrationStatus.Registered or SystemInstanceRegistrationStatus.Unregistered,
            SystemInstanceOperationType.Configure or SystemInstanceOperationType.Start
                or SystemInstanceOperationType.Stop or SystemInstanceOperationType.Restart =>
                instance.RegistrationStatus == SystemInstanceRegistrationStatus.Registered,
            _ => false
        };
    }

    private static bool IsDefinitiveRejection(HttpRequestException exception) =>
        exception.StatusCode is { } statusCode
        && (int)statusCode is >= 400 and < 500
        && statusCode is not (HttpStatusCode.RequestTimeout or (HttpStatusCode)425 or HttpStatusCode.TooManyRequests);
}
