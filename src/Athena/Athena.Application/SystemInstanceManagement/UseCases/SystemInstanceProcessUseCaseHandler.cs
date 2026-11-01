using Athena.Application.Common.Services;
using Athena.Application.SystemInstanceManagement.Commands;
using Athena.Application.SystemInstanceManagement.Contracts;
using Athena.Application.SystemInstanceManagement.Exceptions;
using Athena.Application.SystemInstanceManagement.Queries;

namespace Athena.Application.SystemInstanceManagement.UseCases;

public sealed class SystemInstanceProcessUseCaseHandler(
    SystemInstanceProcessCommandHandler commandHandler,
    SystemInstanceProcessQueryHandler queryHandler,
    ApplicationExceptionService exceptionService)
{
    private readonly SystemInstanceProcessCommandHandler _commandHandler = commandHandler;
    private readonly SystemInstanceProcessQueryHandler _queryHandler = queryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    // Queries

    public Task<IReadOnlyList<SystemInstanceProcessDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetAllAsync(cancellationToken), cancellationToken);

    public Task<SystemInstanceProcessDto?> GetByIdAsync(GetSystemInstanceByIdQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetByIdAsync(query, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> GetStatusAsync(GetSystemInstanceStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetStatusAsync(query, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionHealth> GetHealthAsync(GetSystemInstanceHealthQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetHealthAsync(query, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(GetSystemInstanceLogsQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetLogsAsync(query, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceOperationDto>> GetOperationsAsync(GetSystemInstanceOperationsQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetOperationsAsync(query, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto?> GetOperationAsync(GetSystemInstanceOperationQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetOperationAsync(query, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceOperationDto>> GetPendingOperationsAsync(GetSystemInstancePendingOperationsQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetPendingOperationsAsync(query, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceOperationDto>> GetRunningOperationsAsync(GetSystemInstanceRunningOperationsQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetRunningOperationsAsync(query, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceLifecycleDto>> GetLifecyclesAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetLifecyclesAsync(cancellationToken), cancellationToken);

    public Task<SystemInstanceLifecycleDto?> GetLifecycleAsync(GetSystemInstanceLifecycleQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetLifecycleAsync(query, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceLifecycleDto>> GetByDesiredStateAsync(GetSystemInstancesByDesiredStateQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetByDesiredStateAsync(query, cancellationToken), cancellationToken);

    public Task<IReadOnlyList<SystemInstanceLifecycleDto>> GetByOperationalStatusAsync(GetSystemInstancesByOperationalStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetByOperationalStatusAsync(query, cancellationToken), cancellationToken);

    public Task<bool> HasActiveOperationAsync(HasActiveSystemInstanceOperationQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.HasActiveOperationAsync(query, cancellationToken), cancellationToken);

    // Commands

    public Task<SystemInstanceExecutionStatus> CreateAsync(CreateSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.CreateAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> CreateOperationAsync(CreateSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.CreateOperationAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> RegisterAsync(RegisterSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.RegisterAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> RegisterOperationAsync(RegisterSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.RegisterOperationAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> StartAsync(StartSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.StartAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> StopAsync(StopSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.StopAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> RestartAsync(RestartSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.RestartAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceExecutionStatus> RemoveAsync(RemoveSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.RemoveAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> EnqueueOperationAsync(EnqueueSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.EnqueueOperationAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> RemoveOperationAsync(RemoveSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.RemoveOperationAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> UnregisterOperationAsync(UnregisterSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.UnregisterOperationAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto> ConfigureOperationAsync(ConfigureSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.ConfigureOperationAsync(command, cancellationToken), cancellationToken);

    public Task<SystemInstanceOperationDto?> ResolveOutcomeUnknownOperationAsync(ResolveSystemInstanceOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _commandHandler.ResolveOutcomeUnknownOperationAsync(command, cancellationToken), cancellationToken);

    // Exception handling
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
            return await _exceptionService.ExecuteAsync<T>(() => Task.FromException<T>(new ExecutionAgentTimeoutException(exception)));
        }
    }
}