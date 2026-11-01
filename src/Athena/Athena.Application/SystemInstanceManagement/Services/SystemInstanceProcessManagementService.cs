using System.Runtime.ExceptionServices;
using System.Text.Json;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.Exceptions;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;
using Athena.Application.SystemInstanceManagement.Contracts;
using Athena.Application.SystemInstanceManagement.Interfaces;
using Athena.Application.SystemInstanceManagement.Exceptions;
using ApplicationSystemInstanceNotFoundException = Athena.Application.SystemInstanceManagement.Exceptions.SystemInstanceNotFoundException;

namespace Athena.Application.SystemInstanceManagement.Services;

public sealed class SystemInstanceProcessManagementService(
    ISystemInstanceExecutionAgent executionAgent,
    ISystemInstanceRepository systemInstanceRepository,
    ISystemInstanceLifecycleRepository lifecycleRepository,
    ISystemInstanceOperationRepository operationRepository) : ISystemInstanceProcessCoordinator
{
    private readonly ISystemInstanceExecutionAgent _executionAgent = executionAgent;
    private readonly ISystemInstanceRepository _repository = systemInstanceRepository;
    private readonly ISystemInstanceLifecycleRepository _lifecycleRepository = lifecycleRepository;
    private readonly ISystemInstanceOperationRepository _operationRepository = operationRepository;

    public async Task<IReadOnlyList<SystemInstanceProcessDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var instances = await _repository.GetAllAsync(cancellationToken);
        var results = new List<SystemInstanceProcessDto>(instances.Count);
        foreach (var instance in instances)
            results.Add(ToDto(instance, await GetRequiredLifecycleAsync(instance.Id, cancellationToken)));
        return results;
    }

    public async Task<SystemInstanceProcessDto?> GetByIdAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await FindAsync(systemInstanceId, cancellationToken);
        return instance is null ? null : ToDto(instance, await GetRequiredLifecycleAsync(instance.Id, cancellationToken));
    }

    public async Task<SystemInstanceExecutionStatus> CreateAsync(
        SystemInstanceExecutionCommandRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.SystemId is null || request.WorkloadId is null)
            throw new ArgumentException("SystemId and WorkloadId are required to create a SystemInstance.", nameof(request));

        var id = SystemInstanceId.Create(request.SystemInstanceId);
        if (await _repository.ExistsByIdAsync(id, cancellationToken))
            throw new InvalidOperationException($"SystemInstance '{request.SystemInstanceId}' already exists.");

        var instance = SystemInstance.Create(
            id,
            SystemId.Create(request.SystemId.Value),
            WorkloadId.Create(request.WorkloadId.Value),
            SystemInstanceName.Create(request.Name ?? string.Empty));
        var lifecycle = SystemInstanceLifecycle.Create(instance.Id);
        await _repository.AddAsync(instance, lifecycle, cancellationToken);

        return await RecordOperationAsync(
            instance,
            SystemInstanceOperationType.Create,
            () => _executionAgent.CreateAsync(ToAgentRequest(instance, request.Details), cancellationToken),
            response => ValidateStatus(response, instance.Id.Value),
            static _ => Task.CompletedTask,
            cancellationToken);
    }

    public async Task<SystemInstanceOperationDto> CreateOperationAsync(
        SystemInstanceExecutionCommandRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.SystemId is null || request.WorkloadId is null)
            throw new ArgumentException("SystemId and WorkloadId are required to create a SystemInstance.", nameof(request));

        var id = SystemInstanceId.Create(request.SystemInstanceId);
        if (await _repository.ExistsByIdAsync(id, cancellationToken))
            throw new InvalidOperationException($"SystemInstance '{request.SystemInstanceId}' already exists.");

        var instance = SystemInstance.Create(
            id,
            SystemId.Create(request.SystemId.Value),
            WorkloadId.Create(request.WorkloadId.Value),
            SystemInstanceName.Create(request.Name ?? string.Empty));
        var lifecycle = SystemInstanceLifecycle.Create(instance.Id);
        var operation = SystemInstanceOperation.Create(
            instance.Id,
            SystemInstanceOperationType.Create,
            SerializePayload(request.Details, request.ConfigurationReference));
        await _operationRepository.AddWithRegistryAsync(operation, instance, lifecycle, cancellationToken);
        return ToOperationDto(operation);
    }

    public Task<SystemInstanceOperationDto> RegisterOperationAsync(
        SystemInstanceExecutionCommandRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return EnqueueOperationAsync(
            request.SystemInstanceId,
            SystemInstanceOperationType.Register,
            SerializePayload(request.Details, request.ConfigurationReference),
            cancellationToken);
    }

    public Task<SystemInstanceOperationDto> RemoveOperationAsync(
        Guid systemInstanceId,
        CancellationToken cancellationToken = default) =>
        EnqueueOperationAsync(systemInstanceId, SystemInstanceOperationType.Remove, null, cancellationToken);

    public Task<SystemInstanceOperationDto> UnregisterOperationAsync(
        Guid systemInstanceId,
        CancellationToken cancellationToken = default) =>
        EnqueueOperationAsync(systemInstanceId, SystemInstanceOperationType.Unregister, null, cancellationToken);

    public Task<SystemInstanceOperationDto> ConfigureOperationAsync(
        Guid systemInstanceId,
        string configurationReference,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configurationReference);
        return EnqueueOperationAsync(
            systemInstanceId,
            SystemInstanceOperationType.Configure,
            JsonSerializer.Serialize(new { configurationReference }),
            cancellationToken);
    }

    public async Task<SystemInstanceExecutionStatus> RegisterAsync(
        SystemInstanceExecutionCommandRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var instance = await GetRequiredAsync(request.SystemInstanceId, cancellationToken);
        EnsureCanRegister(instance);

        return await RecordOperationAsync(
            instance,
            SystemInstanceOperationType.Register,
            () => _executionAgent.RegisterAsync(ToAgentRequest(instance, request.Details), cancellationToken),
            response => ValidateStatus(response, instance.Id.Value),
            async _ =>
            {
                instance.Register();
                await _repository.UpdateAsync(instance, CancellationToken.None);
            },
            cancellationToken);
    }

    public Task<SystemInstanceExecutionStatus> StartAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteOperationAsync(id, SystemInstanceOperationType.Start, SystemInstanceDesiredState.Running, _executionAgent.StartAsync, cancellationToken);

    public Task<SystemInstanceExecutionStatus> StopAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteOperationAsync(id, SystemInstanceOperationType.Stop, SystemInstanceDesiredState.Stopped, _executionAgent.StopAsync, cancellationToken);

    public Task<SystemInstanceExecutionStatus> RestartAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteOperationAsync(id, SystemInstanceOperationType.Restart, SystemInstanceDesiredState.Running, _executionAgent.RestartAsync, cancellationToken);

    public async Task<SystemInstanceExecutionStatus> RemoveAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);

        return await RecordOperationAsync(
            instance,
            SystemInstanceOperationType.Remove,
            () => _executionAgent.RemoveAsync(systemInstanceId, cancellationToken),
            response => ValidateStatus(response, systemInstanceId),
            static _ => Task.CompletedTask,
            cancellationToken);
    }

    public async Task<SystemInstanceExecutionStatus> GetStatusAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        var response = await _executionAgent.GetStatusAsync(systemInstanceId, cancellationToken);
        ValidateStatus(response, systemInstanceId);
        var lifecycle = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
        lifecycle.SetObservedOperationalStatus(ParseOperationalStatus(response.State), response.ObservedAtUtc);
        await _lifecycleRepository.UpdateAsync(lifecycle, cancellationToken);
        return response;
    }

    public async Task MarkUnreachableAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        await MarkStatusUnobservableAsync(systemInstanceId, SystemInstanceOperationalStatus.Unreachable, cancellationToken);
    }

    public async Task<bool> SynchronizeStatusAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        if (instance.RegistrationStatus != SystemInstanceRegistrationStatus.Registered)
            return false;

        var lifecycle = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
        var response = await _executionAgent.GetStatusAsync(systemInstanceId, cancellationToken);
        ValidateStatus(response, systemInstanceId);
        var observedStatus = ParseOperationalStatus(response.State);

        lifecycle = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
        var changed = lifecycle.SetObservedOperationalStatus(observedStatus, response.ObservedAtUtc);
        await _lifecycleRepository.UpdateAsync(lifecycle, cancellationToken);
        return changed;
    }

    public async Task<bool> MarkStatusUnobservableAsync(
        Guid systemInstanceId,
        SystemInstanceOperationalStatus status,
        CancellationToken cancellationToken = default)
    {
        if (status is not (SystemInstanceOperationalStatus.Unknown or SystemInstanceOperationalStatus.Unreachable))
            throw new ArgumentOutOfRangeException(nameof(status), "Only Unknown and Unreachable describe an unobservable SystemInstance.");

        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        if (instance.RegistrationStatus != SystemInstanceRegistrationStatus.Registered)
            return false;

        var lifecycle = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
        var changed = lifecycle.SetObservedOperationalStatus(status, DateTime.UtcNow);
        await _lifecycleRepository.UpdateAsync(lifecycle, cancellationToken);
        return changed;
    }

    public async Task<SystemInstanceExecutionHealth> GetHealthAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        await GetRequiredAsync(systemInstanceId, cancellationToken);
        var response = await _executionAgent.GetHealthAsync(systemInstanceId, cancellationToken);
        ValidateHealth(response, systemInstanceId);
        return response;
    }

    public async Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(
        Guid systemInstanceId,
        int tail = 100,
        CancellationToken cancellationToken = default)
    {
        if (tail is < 1 or > 1000)
            throw new ArgumentOutOfRangeException(nameof(tail), "Tail must be between 1 and 1000.");

        await GetRequiredAsync(systemInstanceId, cancellationToken);
        var response = await _executionAgent.GetLogsAsync(systemInstanceId, tail, cancellationToken);
        ValidateLogs(response, systemInstanceId);
        return response;
    }

    public async Task<IReadOnlyList<SystemInstanceOperationDto>> GetOperationsAsync(
        Guid systemInstanceId,
        CancellationToken cancellationToken = default)
    {
        var instanceId = SystemInstanceId.Create(systemInstanceId);
        if (!await _repository.ExistsByIdAsync(instanceId, cancellationToken))
            throw new ApplicationSystemInstanceNotFoundException(systemInstanceId);

        var lifecycle = await GetRequiredLifecycleAsync(instanceId, cancellationToken);
        return (await _operationRepository.GetBySystemInstanceIdAsync(instanceId, cancellationToken))
            .Select(operation => ToOperationDto(operation, lifecycle))
            .ToArray();
    }

    public async Task<SystemInstanceOperationDto> EnqueueOperationAsync(
        Guid systemInstanceId,
        SystemInstanceOperationType operationType,
        string? commandPayloadJson = null,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(operationType))
            throw new ArgumentOutOfRangeException(nameof(operationType), "The SystemInstance operation type is invalid.");
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        switch (operationType)
        {
            case SystemInstanceOperationType.Start:
            case SystemInstanceOperationType.Stop:
            case SystemInstanceOperationType.Restart:
            case SystemInstanceOperationType.Configure:
                EnsureRegistered(instance);
                break;
            case SystemInstanceOperationType.Register:
                EnsureCanRegister(instance);
                break;
            case SystemInstanceOperationType.Unregister:
                EnsureRegistered(instance);
                break;
            case SystemInstanceOperationType.Remove:
                break;
            case SystemInstanceOperationType.Create:
                if (instance.RegistrationStatus != SystemInstanceRegistrationStatus.Unregistered)
                    throw new InvalidOperationException($"SystemInstance '{systemInstanceId}' must be unregistered to process Create.");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operationType));
        }
        var lifecycle = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
        if (await _operationRepository.HasActiveOperationAsync(instance.Id, cancellationToken))
            throw new InvalidOperationException($"SystemInstance '{systemInstanceId}' already has an active operation.");

        if (operationType is SystemInstanceOperationType.Start or SystemInstanceOperationType.Restart)
            lifecycle.SetDesiredState(SystemInstanceDesiredState.Running);
        else if (operationType == SystemInstanceOperationType.Stop)
            lifecycle.SetDesiredState(SystemInstanceDesiredState.Stopped);
        var operation = SystemInstanceOperation.Create(instance.Id, operationType, commandPayloadJson);
        await _operationRepository.AddWithLifecycleAsync(operation, lifecycle, cancellationToken);
        return ToOperationDto(operation);
    }

    private static string? SerializePayload(string? details, string? configurationReference)
    {
        if (!string.IsNullOrWhiteSpace(details))
            throw new ArgumentException("Queued operations do not persist inline details. Store configuration separately and pass a non-secret reference.", nameof(details));
        return string.IsNullOrWhiteSpace(configurationReference)
            ? null
            : JsonSerializer.Serialize(new { configurationReference });
    }

    public async Task<SystemInstanceOperationDto?> GetOperationAsync(
        Guid systemInstanceId,
        Guid operationId,
        CancellationToken cancellationToken = default)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        var operation = await _operationRepository.GetByIdAsync(operationId, cancellationToken);
        if (operation?.SystemInstanceId != instance.Id) return null;
        return ToOperationDto(operation, await GetRequiredLifecycleAsync(instance.Id, cancellationToken));
    }

    public async Task<SystemInstanceOperationDto?> ResolveOutcomeUnknownOperationAsync(
        Guid systemInstanceId,
        Guid operationId,
        string reason,
        CancellationToken cancellationToken = default)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        var operation = await _operationRepository.GetByIdAsync(operationId, cancellationToken);
        if (operation?.SystemInstanceId != instance.Id) return null;

        operation.MarkIndeterminate(reason);
        await _operationRepository.UpdateAsync(operation, cancellationToken);
        return ToOperationDto(operation, await GetRequiredLifecycleAsync(instance.Id, cancellationToken));
    }

    public async Task EnsureOperableAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        EnsureRegistered(instance);
        _ = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
    }

    public async Task<IReadOnlyList<SystemInstanceLifecycleDto>> GetLifecyclesAsync(
    CancellationToken cancellationToken = default)
    {
        var lifecycles = await _lifecycleRepository.GetAllAsync(cancellationToken);
        return lifecycles.Select(ToLifecycleDto).ToArray();
    }

    public async Task<SystemInstanceLifecycleDto?> GetLifecycleAsync(
        Guid systemInstanceId,
        CancellationToken cancellationToken = default)
    {
        var id = SystemInstanceId.Create(systemInstanceId);
        var lifecycle = await _lifecycleRepository.GetByIdAsync(id, cancellationToken);

        return lifecycle is null ? null : ToLifecycleDto(lifecycle);
    }

    public async Task<IReadOnlyList<SystemInstanceLifecycleDto>> GetByDesiredStateAsync(
        SystemInstanceDesiredState state,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(state))
            throw new ArgumentOutOfRangeException(nameof(state));

        var lifecycles = await _lifecycleRepository.GetByDesiredStateAsync(state, cancellationToken);
        return lifecycles.Select(ToLifecycleDto).ToArray();
    }

    public async Task<IReadOnlyList<SystemInstanceLifecycleDto>> GetByOperationalStatusAsync(
        SystemInstanceOperationalStatus status,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(status));

        var lifecycles = await _lifecycleRepository.GetByOperationalStatusAsync(status, cancellationToken);
        return lifecycles.Select(ToLifecycleDto).ToArray();
    }

    public async Task<bool> HasActiveOperationAsync(
        Guid systemInstanceId,
        CancellationToken cancellationToken = default)
    {
        var id = SystemInstanceId.Create(systemInstanceId);

        if (!await _repository.ExistsByIdAsync(id, cancellationToken))
            throw new ApplicationSystemInstanceNotFoundException(systemInstanceId);

        return await _operationRepository.HasActiveOperationAsync(id, cancellationToken);
    }

    //=======================================================================================================================

    private static SystemInstanceLifecycleDto ToLifecycleDto(
        SystemInstanceLifecycle lifecycle) =>
        new(
            lifecycle.Id.Value,
            lifecycle.DesiredState.ToString(),
            lifecycle.OperationalStatus.ToString(),
            lifecycle.CreatedAtUtc,
            lifecycle.UpdatedAtUtc,
            lifecycle.ObservedAtUtc,
            lifecycle.IsDeleted);

    private async Task<SystemInstanceExecutionStatus> ExecuteOperationAsync(
        Guid systemInstanceId,
        SystemInstanceOperationType operationType,
        SystemInstanceDesiredState desiredState,
        Func<Guid, CancellationToken, Task<SystemInstanceExecutionStatus>> operation,
        CancellationToken cancellationToken)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        EnsureRegistered(instance);
        cancellationToken.ThrowIfCancellationRequested();

        var lifecycle = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
        lifecycle.SetDesiredState(desiredState);

        return await RecordOperationAsync(
            instance,
            operationType,
            () => operation(systemInstanceId, cancellationToken),
            response => ValidateStatus(response, systemInstanceId),
            static _ => Task.CompletedTask,
            cancellationToken);
    }

    private async Task<T> RecordOperationAsync<T>(
        SystemInstance instance,
        SystemInstanceOperationType operationType,
        Func<Task<T>> execute,
        Action<T> validate,
        Func<T, Task> applyAcceptedResult,
        CancellationToken cancellationToken)
    {
        var operation = SystemInstanceOperation.Create(instance.Id, operationType);
        var lifecycle = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
        operation.Start();
        await _operationRepository.AddWithLifecycleAsync(operation, lifecycle, cancellationToken);

        T result;
        try
        {
            result = await execute();
            validate(result);
        }
        catch (Exception exception)
        {
            if (IsDefiniteAgentCommandFailure(exception))
                await FailOperationPreservingExceptionAsync(operation, exception);
            ExceptionDispatchInfo.Capture(exception).Throw();
            throw;
        }

        try
        {
            await applyAcceptedResult(result);
        }
        catch (Exception localStateException)
        {
            throw new AggregateException(
                $"Execution agent accepted operation '{operation.Id}', but the corresponding local registry update could not be persisted.",
                localStateException);
        }
        return result;
    }

    private static bool IsDefiniteAgentCommandFailure(Exception exception)
    {
        if (exception is ExecutionAgentNotConfiguredException)
            return true;

        return exception is HttpRequestException { StatusCode: { } statusCode }
            && (int)statusCode is >= 400 and < 500;
    }

    private async Task FailOperationPreservingExceptionAsync(SystemInstanceOperation operation, Exception originalException)
    {
        operation.Fail(originalException.Message);
        try
        {
            await _operationRepository.UpdateAsync(operation, CancellationToken.None);
        }
        catch (Exception persistenceException)
        {
            throw new AggregateException(
                $"Operation '{operation.Id}' failed and its failure status could not be persisted.",
                originalException,
                persistenceException);
        }
    }

    private async Task<SystemInstance?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new SystemInstanceIdRequiredException();
        return await _repository.GetByIdAsync(SystemInstanceId.Create(id), cancellationToken);
    }

    private async Task<SystemInstance> GetRequiredAsync(Guid id, CancellationToken cancellationToken) =>
        await FindAsync(id, cancellationToken) ?? throw new ApplicationSystemInstanceNotFoundException(id);

    private async Task<SystemInstanceLifecycle> GetRequiredLifecycleAsync(SystemInstanceId id, CancellationToken cancellationToken) =>
        await _lifecycleRepository.GetByIdAsync(id, cancellationToken)
        ?? throw new InvalidOperationException($"SystemInstance lifecycle '{id.Value}' does not exist.");

    private static void EnsureCanRegister(SystemInstance instance)
    {
        if (instance.RegistrationStatus == SystemInstanceRegistrationStatus.Registered)
            throw new SystemInstanceAlreadyRegisteredException(instance.Id.Value);
        if (instance.RegistrationStatus == SystemInstanceRegistrationStatus.Disabled)
            throw new SystemInstanceDisabledCannotBeModifiedException(instance.Id.Value);
    }

    private static void EnsureRegistered(SystemInstance instance)
    {
        if (instance.RegistrationStatus == SystemInstanceRegistrationStatus.Unregistered)
            throw new SystemInstanceNotRegisteredException(instance.Id.Value);
        if (instance.RegistrationStatus == SystemInstanceRegistrationStatus.Disabled)
            throw new SystemInstanceDisabledCannotBeModifiedException(instance.Id.Value);
    }

    private static SystemInstanceExecutionCommandRequest ToAgentRequest(SystemInstance instance, string? details) =>
        new(instance.Id.Value, instance.SystemId.Value, instance.WorkloadId.Value, instance.Name.Value, details);

    private static SystemInstanceProcessDto ToDto(SystemInstance instance, SystemInstanceLifecycle lifecycle) =>
        new(instance.Id.Value, instance.SystemId.Value, instance.WorkloadId.Value, instance.Name.Value,
            instance.RegistrationStatus.ToString(), lifecycle.DesiredState.ToString(), lifecycle.OperationalStatus.ToString(),
            instance.CreatedAtUtc, instance.UpdatedAtUtc, lifecycle.ObservedAtUtc);

    private static SystemInstanceOperationDto ToOperationDto(
        SystemInstanceOperation operation,
        SystemInstanceLifecycle? lifecycle = null) =>
        new(
            operation.Id,
            operation.SystemInstanceId.Value,
            operation.Type.ToString(),
            operation.Status.ToString(),
            operation.CreatedAtUtc,
            operation.StartedAtUtc,
            operation.CompletedAtUtc,
            operation.ErrorMessage,
            operation.AgentAcceptedAtUtc,
            operation.AgentCompletedAtUtc,
            operation.LeaseExpiresAtUtc,
            operation.AttemptCount,
            GetConvergenceStatus(operation, lifecycle),
            operation.MaxAttempts);

    private static string? GetConvergenceStatus(
        SystemInstanceOperation operation,
        SystemInstanceLifecycle? lifecycle)
    {
        if (operation.Status != SystemInstanceOperationStatus.OutcomeUnknown
            || lifecycle?.ObservedAtUtc is not { } observedAtUtc
            || observedAtUtc < DateTime.UtcNow.AddMinutes(-3)
            || operation.Type == SystemInstanceOperationType.Restart)
            return null;

        var expected = operation.Type == SystemInstanceOperationType.Start
            ? SystemInstanceOperationalStatus.Running
            : SystemInstanceOperationalStatus.Stopped;
        return lifecycle.OperationalStatus == expected ? "ConvergedUncorrelated" : "NotConverged";
    }

    private static void ValidateStatus(SystemInstanceExecutionStatus status, Guid requestedId)
    {
        if (status is null)
            throw new ExecutionAgentResponseException("Execution agent returned a null status response.");
        if (status.SystemInstanceId != requestedId)
            throw new ExecutionAgentResponseException($"Execution agent returned status for SystemInstance '{status.SystemInstanceId}' when '{requestedId}' was requested.");
        if (string.IsNullOrWhiteSpace(status.State))
            throw new ExecutionAgentResponseException("Execution agent returned an empty operational state.");
        if (status.ObservedAtUtc == default || status.ObservedAtUtc.Kind != DateTimeKind.Utc)
            throw new ExecutionAgentResponseException("Execution agent returned an invalid ObservedAtUtc timestamp.");
        var state = ParseOperationalStatus(status.State);
        if (status.IsRunning != (state == SystemInstanceOperationalStatus.Running))
            throw new ExecutionAgentResponseException("Execution agent returned inconsistent State and IsRunning values.");
    }

    private static void ValidateHealth(SystemInstanceExecutionHealth health, Guid requestedId)
    {
        if (health is null)
            throw new ExecutionAgentResponseException("Execution agent returned a null health response.");
        if (health.SystemInstanceId != requestedId)
            throw new ExecutionAgentResponseException($"Execution agent returned health for SystemInstance '{health.SystemInstanceId}' when '{requestedId}' was requested.");
        if (string.IsNullOrWhiteSpace(health.Status))
            throw new ExecutionAgentResponseException("Execution agent returned an empty health status.");
        if (health.CheckedAtUtc == default || health.CheckedAtUtc.Kind != DateTimeKind.Utc)
            throw new ExecutionAgentResponseException("Execution agent returned an invalid CheckedAtUtc timestamp.");
    }

    private static void ValidateLogs(IReadOnlyList<SystemInstanceExecutionLogEntry> logs, Guid systemInstanceId)
    {
        if (logs is null)
            throw new ExecutionAgentResponseException($"Execution agent returned null logs for SystemInstance '{systemInstanceId}'.");
        for (var index = 0; index < logs.Count; index++)
        {
            var entry = logs[index];
            if (entry is null)
                throw new ExecutionAgentResponseException($"Execution agent returned a null log entry at index {index} for SystemInstance '{systemInstanceId}'.");
            if (entry.TimestampUtc == default || entry.TimestampUtc.Kind != DateTimeKind.Utc)
                throw new ExecutionAgentResponseException($"Execution agent returned an invalid log timestamp at index {index}.");
            if (string.IsNullOrWhiteSpace(entry.Stream) || entry.Message is null)
                throw new ExecutionAgentResponseException($"Execution agent returned invalid log data at index {index}.");
        }
    }

    private static SystemInstanceOperationalStatus ParseOperationalStatus(string state) =>
        state.Trim().ToLowerInvariant() switch
        {
            "running" or "active" => SystemInstanceOperationalStatus.Running,
            "stopped" or "exited" => SystemInstanceOperationalStatus.Stopped,
            "starting" => SystemInstanceOperationalStatus.Starting,
            "stopping" => SystemInstanceOperationalStatus.Stopping,
            "failed" or "error" => SystemInstanceOperationalStatus.Failed,
            "unreachable" or "unknown" => SystemInstanceOperationalStatus.Unreachable,
            _ => throw new ExecutionAgentResponseException($"Execution agent returned unsupported operational state '{state}'.")
        };
}
