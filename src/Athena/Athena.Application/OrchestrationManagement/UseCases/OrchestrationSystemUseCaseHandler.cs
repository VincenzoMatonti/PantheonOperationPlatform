using Athena.Application.Common.Services;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Queries;

namespace Athena.Application.OrchestrationManagement.UseCases;

public class OrchestrationSystemInstanceUseCaseHandler(OrchestrationSystemInstanceCommandHandler commandHandler, OrchestrationSystemInstanceQueryHandler queryHandler, ApplicationExceptionService exceptionService)
{
    private readonly OrchestrationSystemInstanceCommandHandler _commandHandler = commandHandler;
    private readonly OrchestrationSystemInstanceQueryHandler _queryHandler = queryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);

    // =============================================================================================================
    // USE CASE COMMANDS - ORCHESTRATION SYSTEM
    public Task<OrchestrationSystemInstanceDto> CreateOrchestrationSystemInstanceAsync(CreateOrchestrationSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.CreateOrchestrationSystemInstanceAsync(command, cancellationToken));

    public Task<OrchestrationSystemInstanceDto> DeleteOrchestrationSystemInstanceAsync(DeleteOrchestrationSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.DeleteOrchestrationSystemInstanceAsync(command, cancellationToken) ?? throw new OrchestrationSystemInstanceNotFoundException(command.Id));

    public Task<OrchestrationSystemInstanceDto> RestoreOrchestrationSystemInstanceAsync(RestoreOrchestrationSystemInstanceCommand command, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _commandHandler.RestoreOrchestrationSystemInstanceAsync(command, cancellationToken) ?? throw new OrchestrationSystemInstanceNotFoundException(command.Id));

    // =============================================================================================================
    // USE CASE QUERIES - ORCHESTRATION SYSTEM

    public Task<OrchestrationSystemInstanceDto> GetOrchestrationSystemInstanceByIdAsync(GetOrchestrationSystemInstanceByIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () => await _queryHandler.GetOrchestrationSystemInstanceByIdAsync(query, cancellationToken) ?? throw new OrchestrationSystemInstanceNotFoundException(query.Id));

    public Task<List<OrchestrationSystemInstanceDto>> GetAllOrchestrationSystemInstancesAsync(GetOrchestrationSystemInstancesQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _queryHandler.GetAllOrchestrationSystemInstancesAsync(query, cancellationToken));
}
