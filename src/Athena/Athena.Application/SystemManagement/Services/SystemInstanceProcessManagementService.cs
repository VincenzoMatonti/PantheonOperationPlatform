using System.Runtime.ExceptionServices;
using Athena.Application.SystemManagement.Contracts;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.Exceptions;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Application.SystemManagement.Services;

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
        await _repository.AddAsync(instance, cancellationToken);
        await _lifecycleRepository.AddAsync(SystemInstanceLifecycle.Create(instance.Id), cancellationToken);

        return await RecordOperationAsync(
            instance,
            SystemInstanceOperationType.Create,
            () => _executionAgent.CreateAsync(ToAgentRequest(instance, request.Details), cancellationToken),
            response => ValidateStatus(response, instance.Id.Value),
            async response =>
            {
                var lifecycle = await GetRequiredLifecycleAsync(instance.Id, CancellationToken.None);
                lifecycle.SetOperationalStatus(ParseOperationalStatus(response.State));
                await _lifecycleRepository.UpdateAsync(lifecycle, CancellationToken.None);
            },
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
            async response =>
            {
                instance.Register();
                var lifecycle = await GetRequiredLifecycleAsync(instance.Id, CancellationToken.None);
                lifecycle.SetOperationalStatus(ParseOperationalStatus(response.State));
                await _repository.UpdateAsync(instance, CancellationToken.None);
                await _lifecycleRepository.UpdateAsync(lifecycle, CancellationToken.None);
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
            async _ =>
            {
                var lifecycle = await GetRequiredLifecycleAsync(instance.Id, CancellationToken.None);
                lifecycle.Delete();
                await _lifecycleRepository.UpdateAsync(lifecycle, CancellationToken.None);
                instance.Delete();
                await _repository.UpdateAsync(instance, CancellationToken.None);
            },
            cancellationToken);
    }

    public async Task<SystemInstanceExecutionStatus> GetStatusAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        var response = await _executionAgent.GetStatusAsync(systemInstanceId, cancellationToken);
        ValidateStatus(response, systemInstanceId);
        var lifecycle = await GetRequiredLifecycleAsync(instance.Id, CancellationToken.None);
        lifecycle.SetOperationalStatus(ParseOperationalStatus(response.State));
        await _lifecycleRepository.UpdateAsync(lifecycle, CancellationToken.None);
        return response;
    }

    public async Task MarkUnreachableAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        var lifecycle = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
        lifecycle.SetOperationalStatus(SystemInstanceOperationalStatus.Unreachable);
        await _lifecycleRepository.UpdateAsync(lifecycle, cancellationToken);
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
            throw new SystemInstanceNotFoundException(systemInstanceId);

        return (await _operationRepository.GetBySystemInstanceIdAsync(instanceId, cancellationToken))
            .Select(operation => new SystemInstanceOperationDto(
                operation.Id,
                operation.SystemInstanceId.Value,
                operation.Type.ToString(),
                operation.Status.ToString(),
                operation.CreatedAtUtc,
                operation.StartedAtUtc,
                operation.CompletedAtUtc,
                operation.ErrorMessage))
            .ToArray();
    }

    public async Task EnsureOperableAsync(Guid systemInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await GetRequiredAsync(systemInstanceId, cancellationToken);
        EnsureRegistered(instance);
        _ = await GetRequiredLifecycleAsync(instance.Id, cancellationToken);
    }

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
        await _lifecycleRepository.UpdateAsync(lifecycle, cancellationToken);

        return await RecordOperationAsync(
            instance,
            operationType,
            () => operation(systemInstanceId, cancellationToken),
            response => ValidateStatus(response, systemInstanceId),
            async response =>
            {
                var currentLifecycle = await GetRequiredLifecycleAsync(instance.Id, CancellationToken.None);
                currentLifecycle.SetOperationalStatus(ParseOperationalStatus(response.State));
                await _lifecycleRepository.UpdateAsync(currentLifecycle, CancellationToken.None);
            },
            cancellationToken);
    }

    private async Task<T> RecordOperationAsync<T>(
        SystemInstance instance,
        SystemInstanceOperationType operationType,
        Func<Task<T>> execute,
        Action<T> validate,
        Func<T, Task> applyResult,
        CancellationToken cancellationToken)
    {
        var operation = SystemInstanceOperation.Create(instance.Id, operationType);
        await _operationRepository.AddAsync(operation, cancellationToken);
        operation.Start();
        await _operationRepository.UpdateAsync(operation, CancellationToken.None);

        T result;
        try
        {
            result = await execute();
            validate(result);
        }
        catch (Exception exception)
        {
            await FailOperationPreservingExceptionAsync(operation, exception);
            ExceptionDispatchInfo.Capture(exception).Throw();
            throw;
        }

        try
        {
            await applyResult(result);
        }
        catch (Exception persistenceException)
        {
            await CompleteOperationPreservingExceptionAsync(operation, persistenceException);
            ExceptionDispatchInfo.Capture(persistenceException).Throw();
            throw;
        }

        operation.Complete();
        await _operationRepository.UpdateAsync(operation, CancellationToken.None);
        return result;
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

    private async Task CompleteOperationPreservingExceptionAsync(SystemInstanceOperation operation, Exception originalException)
    {
        operation.Complete();
        try
        {
            await _operationRepository.UpdateAsync(operation, CancellationToken.None);
        }
        catch (Exception persistenceException)
        {
            throw new AggregateException(
                $"The remote operation '{operation.Id}' succeeded, but local state persistence and operation completion both failed.",
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
        await FindAsync(id, cancellationToken) ?? throw new SystemInstanceNotFoundException(id);

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
            instance.CreatedAtUtc, instance.UpdatedAtUtc);

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
