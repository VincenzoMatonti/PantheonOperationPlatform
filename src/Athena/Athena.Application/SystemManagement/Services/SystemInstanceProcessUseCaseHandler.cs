using Athena.Application.Common.Services;
using Athena.Application.SystemManagement.Contracts;

namespace Athena.Application.SystemManagement.Services;

public sealed class SystemInstanceProcessUseCaseHandler(
    SystemInstanceProcessManagementService service,
    ApplicationExceptionService exceptionService)
{
    private readonly SystemInstanceProcessManagementService _service = service;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    public Task<IReadOnlyList<SystemInstanceProcessDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetAllAsync(cancellationToken), cancellationToken);

    public Task<SystemInstanceProcessDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetByIdAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> CreateAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.CreateAsync(request, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> RegisterAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.RegisterAsync(request, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> StartAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.StartAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> StopAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.StopAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> RestartAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.RestartAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> RemoveAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.RemoveAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> GetStatusAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetStatusAsync(id, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionHealth> GetHealthAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetHealthAsync(id, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(Guid id, int tail = 100, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetLogsAsync(id, tail, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceOperationDto>> GetOperationsAsync(Guid id, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _service.GetOperationsAsync(id, cancellationToken), cancellationToken);

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
