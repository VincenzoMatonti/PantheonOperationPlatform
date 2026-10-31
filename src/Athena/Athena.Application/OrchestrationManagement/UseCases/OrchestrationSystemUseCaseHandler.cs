using Athena.Application.Common.Services;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Queries;

namespace Athena.Application.OrchestrationManagement.UseCases;

public class OrchestrationSystemUseCaseHandler(OrchestrationSystemCommandHandler commandHandler, OrchestrationSystemQueryHandler queryHandler, ApplicationExceptionService exceptionService)
{
    private readonly OrchestrationSystemCommandHandler _commandHandler = commandHandler;
    private readonly OrchestrationSystemQueryHandler _queryHandler = queryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);

    // =============================================================================================================
    // USE CASE COMMANDS - ORCHESTRATION SYSTEM
    public Task<OrchestrationSystemDto> CreateOrchestrationSystemAsync(CreateOrchestrationSystemCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.CreateOrchestrationSystemAsync(command, cancellationToken));

    public Task<OrchestrationSystemDto> DeleteOrchestrationSystemAsync(DeleteOrchestrationSystemCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DeleteOrchestrationSystemAsync(command, cancellationToken) ?? throw new OrchestrationSystemNotFoundException(command.Id));

    public Task<OrchestrationSystemDto> RestoreOrchestrationSystemAsync(RestoreOrchestrationSystemCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.RestoreOrchestrationSystemAsync(command, cancellationToken) ?? throw new OrchestrationSystemNotFoundException(command.Id));

    // =============================================================================================================
    // USE CASE QUERIES - ORCHESTRATION SYSTEM

    public Task<OrchestrationSystemDto> GetOrchestrationSystemByIdAsync(GetOrchestrationSystemByIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetOrchestrationSystemByIdAsync(query, cancellationToken) ?? throw new OrchestrationSystemNotFoundException(query.Id));

    public Task<List<OrchestrationSystemDto>> GetAllOrchestrationSystemsAsync(GetOrchestrationSystemsQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _queryHandler.GetAllOrchestrationSystemsAsync(query, cancellationToken));
}
