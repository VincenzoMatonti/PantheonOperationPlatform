using Athena.Application.Common.Services;
using Athena.Application.SystemManagement.Commands;
using Athena.Application.SystemManagement.Contracts;
using Athena.Application.SystemManagement.Queries;
using Athena.Domain.System.Management.Enums;

namespace Athena.Application.SystemManagement.Services;

public sealed class SystemInstanceProcessUseCaseHandler(
    SystemInstanceProcessManagementService service,
    ApplicationExceptionService exceptionService)
{
    private readonly SystemInstanceProcessManagementService _service = service;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    public Task<IReadOnlyList<SystemInstanceProcessDto>> GetAllAsync(GetSystemInstancesQuery query, CancellationToken cancellationToken = default) =>
        GetAllAsync(cancellationToken);

    public Task<SystemInstanceProcessDto?> GetByIdAsync(GetSystemInstanceByIdQuery query, CancellationToken cancellationToken = default) =>
        GetByIdAsync(query.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceOperationDto> CreateAsync(CreateSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        CreateOperationAsync(command.Request, cancellationToken);

    public Task<SystemInstanceOperationDto> RegisterAsync(RegisterSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        RegisterOperationAsync(command.Request, cancellationToken);

    public Task<SystemInstanceOperationDto> EnqueueOperationAsync(EnqueueSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        EnqueueOperationAsync(command.SystemInstanceId, command.Type, cancellationToken);

    public Task<SystemInstanceOperationDto> ConfigureAsync(ConfigureSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ConfigureOperationAsync(command.SystemInstanceId, command.ConfigurationReference, cancellationToken);

    public Task<SystemInstanceOperationDto?> ResolveOutcomeUnknownOperationAsync(ResolveSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        ResolveOutcomeUnknownOperationAsync(command.SystemInstanceId, command.OperationId, command.Reason, cancellationToken);

    public Task<SystemInstanceExecutionStatus> GetStatusAsync(GetSystemInstanceStatusQuery query, CancellationToken cancellationToken = default) =>
        GetStatusAsync(query.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceExecutionHealth> GetHealthAsync(GetSystemInstanceHealthQuery query, CancellationToken cancellationToken = default) =>
        GetHealthAsync(query.SystemInstanceId, cancellationToken);

    public Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(GetSystemInstanceLogsQuery query, CancellationToken cancellationToken = default) =>
        GetLogsAsync(query.SystemInstanceId, query.Tail, cancellationToken);

    public Task<IReadOnlyList<SystemInstanceOperationDto>> GetOperationsAsync(GetSystemInstanceOperationsQuery query, CancellationToken cancellationToken = default) =>
        GetOperationsAsync(query.SystemInstanceId, cancellationToken);

    public Task<SystemInstanceOperationDto?> GetOperationAsync(GetSystemInstanceOperationQuery query, CancellationToken cancellationToken = default) =>
        GetOperationAsync(query.SystemInstanceId, query.OperationId, cancellationToken);

    public Task<IReadOnlyList<SystemInstanceProcessDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetAllAsync(cancellationToken), cancellationToken);

    public Task<SystemInstanceProcessDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetByIdAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> CreateAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.CreateAsync(request, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> CreateOperationAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.CreateOperationAsync(request, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> RegisterAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.RegisterAsync(request, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> RegisterOperationAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.RegisterOperationAsync(request, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> StartAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.StartAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> StopAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.StopAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> RestartAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.RestartAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> RemoveAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.RemoveAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> RemoveOperationAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.RemoveOperationAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> UnregisterOperationAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.UnregisterOperationAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> ConfigureOperationAsync(Guid id, string configurationReference, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.ConfigureOperationAsync(id, configurationReference, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> GetStatusAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetStatusAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionHealth> GetHealthAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetHealthAsync(id, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(Guid id, int tail = 100, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetLogsAsync(id, tail, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceOperationDto>> GetOperationsAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetOperationsAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> EnqueueOperationAsync(
        Guid id,
        SystemInstanceOperationType type,
        CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.EnqueueOperationAsync(id, type, cancellationToken: cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto?> GetOperationAsync(
        Guid id,
        Guid operationId,
        CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetOperationAsync(id, operationId, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto?> ResolveOutcomeUnknownOperationAsync(
        Guid id,
        Guid operationId,
        string reason,
        CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.ResolveOutcomeUnknownOperationAsync(id, operationId, reason, cancellationToken), cancellationToken);

    private async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken)
    {
        try
        {
            return await _exceptionService.ExecuteAsync(operation);
        }
        catch (HttpRequestException exception)
        {
            return await _exceptionService.ExecuteAsync<T>(() => Task.FromException<T>(new ExecutionAgentRequestException(exception)));
        }
        catch (TaskCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            return await _exceptionService.ExecuteAsync<T>(() => Task.FromException<T>(
                new ExecutionAgentTimeoutException(exception)));
        }
    }
}
