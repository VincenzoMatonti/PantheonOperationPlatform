using Athena.Application.Common.Services;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Queries;

namespace Athena.Application.OrchestrationManagement.UseCases;

public class OrchestrationLifecycleUseCaseHandler(
    OrchestrationLifecycleCommandHandler commandHandler,
    OrchestrationLifecycleQueryHandler queryHandler,
    ApplicationExceptionService exceptionService)
{
    private readonly OrchestrationLifecycleCommandHandler _commandHandler = commandHandler;
    private readonly OrchestrationLifecycleQueryHandler _queryHandler = queryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);

    // =============================================================================================================
    // USE CASE COMMANDS - ORCHESTRATION LIFECYCLE
    public Task<OrchestrationLifecycleDto> CreateOrchestrationLifecycleAsync(Guid orchestrationId, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var query = new GetOrchestrationLifecycleByIdQuery { OrchestrationId = orchestrationId };
        var existing = await _queryHandler.GetOrchestrationLifecycleByIdAsync(query, cancellationToken);
        if (existing is not null) throw new OrchestrationLifecycleAlreadyExistsException(orchestrationId);
        return await _commandHandler.CreateOrchestrationLifecycleAsync(orchestrationId, cancellationToken);
    });

    public Task<OrchestrationLifecycleDto> SetOrchestrationDesiredStateAsync(SetOrchestrationDesiredStateCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(command.DesiredState)) throw new InvalidDesiredStateException(command.DesiredState.ToString());
        return await _commandHandler.SetOrchestrationDesiredStateAsync(command, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId);
    });

    public Task<OrchestrationLifecycleDto> MarkOrchestrationStartingAsync(MarkOrchestrationStartingCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkOrchestrationStartingAsync(command, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId));

    public Task<OrchestrationLifecycleDto> MarkOrchestrationRunningAsync(MarkOrchestrationRunningCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkOrchestrationRunningAsync(command, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId));

    public Task<OrchestrationLifecycleDto> MarkOrchestrationStoppingAsync(MarkOrchestrationStoppingCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkOrchestrationStoppingAsync(command, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId));

    public Task<OrchestrationLifecycleDto> MarkOrchestrationStoppedAsync(MarkOrchestrationStoppedCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkOrchestrationStoppedAsync(command, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId));

    public Task<OrchestrationLifecycleDto> MarkOrchestrationFailedAsync(MarkOrchestrationFailedCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkOrchestrationFailedAsync(command, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId));

    public Task<OrchestrationLifecycleDto> MarkOrchestrationOperationalStatusUnknownAsync(MarkOrchestrationOperationalStatusUnknownCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.MarkOrchestrationOperationalStatusUnknownAsync(command, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId));

    public Task<OrchestrationLifecycleDto> DeleteOrchestrationLifecycleAsync(DeleteOrchestrationLifecycleCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DeleteOrchestrationLifecycleAsync(command, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId));

    // =============================================================================================================
    // USE CASE QUERIES - ORCHESTRATION LIFECYCLE
    public Task<OrchestrationLifecycleDto> GetOrchestrationLifecycleByIdAsync(GetOrchestrationLifecycleByIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetOrchestrationLifecycleByIdAsync(query, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(query.OrchestrationId));

    public Task<List<OrchestrationLifecycleDto>> GetAllOrchestrationLifecyclesAsync(CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _queryHandler.GetAllOrchestrationLifecyclesAsync(cancellationToken));

    public Task<List<OrchestrationLifecycleDto>> GetOrchestrationLifecyclesByDesiredStateAsync(GetOrchestrationLifecyclesByDesiredStateQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.State)) throw new InvalidDesiredStateException(query.State.ToString());
        return await _queryHandler.GetOrchestrationLifecyclesByDesiredStateAsync(query, cancellationToken);
    });

    public Task<List<OrchestrationLifecycleDto>> GetOrchestrationLifecyclesByOperationalStatusAsync(GetOrchestrationLifecyclesByOperationalStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.Status)) throw new InvalidOperationalStatusException(query.Status.ToString());
        return await _queryHandler.GetOrchestrationLifecyclesByOperationalStatusAsync(query, cancellationToken);
    });

    public Task<List<OrchestrationLifecycleDto>> GetDeletedOrchestrationLifecyclesAsync(CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _queryHandler.GetDeletedOrchestrationLifecyclesAsync(cancellationToken));
}
