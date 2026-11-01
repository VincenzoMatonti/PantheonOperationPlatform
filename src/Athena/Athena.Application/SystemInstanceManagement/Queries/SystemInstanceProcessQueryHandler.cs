using Athena.Application.SystemInstanceManagement.Contracts;
using Athena.Application.SystemInstanceManagement.Services;

namespace Athena.Application.SystemInstanceManagement.Queries;

public sealed class SystemInstanceProcessQueryHandler(SystemInstanceProcessManagementService service)
{
    private readonly SystemInstanceProcessManagementService _service = service;

    public Task<IReadOnlyList<SystemInstanceProcessDto>> GetAllAsync(CancellationToken cancellationToken = default) => _service.GetAllAsync(cancellationToken);

    public Task<SystemInstanceProcessDto?> GetByIdAsync(GetSystemInstanceByIdQuery query, CancellationToken cancellationToken = default) =>
        _service.GetByIdAsync(query.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceExecutionStatus> GetStatusAsync(GetSystemInstanceStatusQuery query, CancellationToken cancellationToken = default) =>
        _service.GetStatusAsync(query.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceExecutionHealth> GetHealthAsync(GetSystemInstanceHealthQuery query, CancellationToken cancellationToken = default) =>
        _service.GetHealthAsync(query.SystemInstanceId, cancellationToken);

    public Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(GetSystemInstanceLogsQuery query, CancellationToken cancellationToken = default) =>
        _service.GetLogsAsync(query.SystemInstanceId, query.Tail, cancellationToken);

    public Task<IReadOnlyList<SystemInstanceOperationDto>> GetOperationsAsync(GetSystemInstanceOperationsQuery query, CancellationToken cancellationToken = default) =>
        _service.GetOperationsAsync(query.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceOperationDto?> GetOperationAsync(GetSystemInstanceOperationQuery query, CancellationToken cancellationToken = default) =>
        _service.GetOperationAsync(query.SystemInstanceId, query.OperationId, cancellationToken);

    public async Task<IReadOnlyList<SystemInstanceOperationDto>> GetPendingOperationsAsync(GetSystemInstancePendingOperationsQuery query, CancellationToken cancellationToken = default)
    {
        var operations = await _service.GetOperationsAsync(query.SystemInstanceId, cancellationToken);
        return [.. operations.Where(operation => operation.Status == "Pending")];
    }

    public async Task<IReadOnlyList<SystemInstanceOperationDto>> GetRunningOperationsAsync(GetSystemInstanceRunningOperationsQuery query, CancellationToken cancellationToken = default)
    {
        var operations = await _service.GetOperationsAsync(query.SystemInstanceId, cancellationToken);
        return [.. operations.Where(operation => operation.Status == "Running")];
    }

    public Task<IReadOnlyList<SystemInstanceLifecycleDto>> GetLifecyclesAsync(CancellationToken cancellationToken = default) =>
        _service.GetLifecyclesAsync(cancellationToken);

    public Task<SystemInstanceLifecycleDto?> GetLifecycleAsync(GetSystemInstanceLifecycleQuery query, CancellationToken cancellationToken = default) =>
        _service.GetLifecycleAsync(query.SystemInstanceId, cancellationToken);

    public Task<IReadOnlyList<SystemInstanceLifecycleDto>> GetByDesiredStateAsync(GetSystemInstancesByDesiredStateQuery query, CancellationToken cancellationToken = default) =>
        _service.GetByDesiredStateAsync(query.DesiredState, cancellationToken);

    public Task<IReadOnlyList<SystemInstanceLifecycleDto>> GetByOperationalStatusAsync(GetSystemInstancesByOperationalStatusQuery query, CancellationToken cancellationToken = default) =>
        _service.GetByOperationalStatusAsync(query.OperationalStatus, cancellationToken);

    public Task<bool> HasActiveOperationAsync(HasActiveSystemInstanceOperationQuery query, CancellationToken cancellationToken = default) =>
        _service.HasActiveOperationAsync(query.SystemInstanceId, cancellationToken);
}