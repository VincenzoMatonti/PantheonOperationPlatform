using Athena.Application.Common.Services;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.SystemManagement.Commands;
using Athena.Application.SystemManagement.Enums;
using Athena.Application.SystemManagement.UseCases;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationRegistry.ValueObjects;

namespace Athena.Application.OrchestrationManagement.UseCases;

public class OrchestrationLifecycleOperationUseCaseHandler(
    OrchestrationLifecycleOperationCommandHandler commandHandler,
    OrchestrationLifecycleOperationQueryHandler queryHandler,
    OrchestrationLifecycleQueryHandler lifecycleQueryHandler,
    ApplicationExceptionService exceptionService,
    SystemManagementUseCaseHandler systemManagementUseCaseHandler,
    IOrchestrationSystemRepository orchestrationSystemRepository,
    OrchestrationLifecycleUseCaseHandler orchestrationLifecycleUseCaseHandler)
{
    private readonly OrchestrationLifecycleOperationCommandHandler _commandHandler = commandHandler;
    private readonly OrchestrationLifecycleOperationQueryHandler _queryHandler = queryHandler;
    private readonly OrchestrationLifecycleQueryHandler _lifecycleQueryHandler = lifecycleQueryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;
    private readonly SystemManagementUseCaseHandler _systemManagementUseCaseHandler = systemManagementUseCaseHandler;
    private readonly IOrchestrationSystemRepository _orchestrationSystemRepository = orchestrationSystemRepository;
    private readonly OrchestrationLifecycleUseCaseHandler _orchestrationLifecycleUseCaseHandler = orchestrationLifecycleUseCaseHandler;
    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);

    // =============================================================================================================
    // USE CASE COMMANDS - ORCHESTRATION LIFECYCLE OPERATIONS
    public Task<OrchestrationLifecycleOperationDto> CreateOrchestrationLifecycleOperationAsync(
        CreateOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(command.Type)) throw new InvalidOperationTypeException(command.Type.ToString());
            var existingOperationQuery = new GetOrchestrationLifecycleOperationByIdQuery { Id = command.Id };
            var existingOperation = await _queryHandler.GetOrchestrationLifecycleOperationByIdAsync(existingOperationQuery, cancellationToken);
            if (existingOperation is not null) throw new OrchestrationLifecycleOperationAlreadyExistsException(command.Id);
            var lifecycleQuery = new GetOrchestrationLifecycleByIdQuery { OrchestrationId = command.OrchestrationId };
            _ = await _lifecycleQueryHandler.GetOrchestrationLifecycleByIdAsync(lifecycleQuery, cancellationToken) ??
            throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId);
            return await _commandHandler.CreateOrchestrationLifecycleOperationAsync(command, cancellationToken) ??
            throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId);
        });

    public Task<OrchestrationLifecycleOperationDto> StartOrchestrationLifecycleOperationAsync(
        StartOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () => await _commandHandler.StartOrchestrationLifecycleOperationAsync(command, cancellationToken) ??
            throw new OrchestrationLifecycleOperationNotFoundException(command.Id));

    public Task<OrchestrationLifecycleOperationDto> CompleteOrchestrationLifecycleOperationAsync(
        CompleteOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () => await _commandHandler.CompleteOrchestrationLifecycleOperationAsync(command, cancellationToken) ??
            throw new OrchestrationLifecycleOperationNotFoundException(command.Id));

    public Task<OrchestrationLifecycleOperationDto> FailOrchestrationLifecycleOperationAsync(
        FailOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () => await _commandHandler.FailOrchestrationLifecycleOperationAsync(command, cancellationToken) ??
            throw new OrchestrationLifecycleOperationNotFoundException(command.Id));

    public Task<OrchestrationLifecycleOperationDto> CancelOrchestrationLifecycleOperationAsync(
        CancelOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () => await _commandHandler.CancelOrchestrationLifecycleOperationAsync(command, cancellationToken) ??
            throw new OrchestrationLifecycleOperationNotFoundException(command.Id));

    public Task<OrchestrationLifecycleOperationDto> DeleteOrchestrationLifecycleOperationAsync(
        DeleteOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () => await _commandHandler.DeleteOrchestrationLifecycleOperationAsync(command, cancellationToken) ??
            throw new OrchestrationLifecycleOperationNotFoundException(command.Id));

    // =============================================================================================================
    // USE CASE QUERIES - ORCHESTRATION LIFECYCLE OPERATIONS
    public Task<OrchestrationLifecycleOperationDto> GetOrchestrationLifecycleOperationByIdAsync(
        GetOrchestrationLifecycleOperationByIdQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () => await _queryHandler.GetOrchestrationLifecycleOperationByIdAsync(query, cancellationToken) ??
            throw new OrchestrationLifecycleOperationNotFoundException(query.Id));

    public Task<List<OrchestrationLifecycleOperationDto>> GetAllOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetAllOrchestrationLifecycleOperationsAsync(cancellationToken));

    public Task<List<OrchestrationLifecycleOperationDto>> GetOrchestrationLifecycleOperationsByOrchestrationIdAsync(
        GetOrchestrationLifecycleOperationsByOrchestrationIdQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetOrchestrationLifecycleOperationsByOrchestrationIdAsync(query, cancellationToken));

    public Task<List<OrchestrationLifecycleOperationDto>> GetOrchestrationLifecycleOperationsByTypeAsync(
        GetOrchestrationLifecycleOperationsByTypeQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(query.Type)) throw new InvalidOperationTypeException(query.Type.ToString());
            return await _queryHandler.GetOrchestrationLifecycleOperationsByTypeAsync(query, cancellationToken);
        });

    public Task<List<OrchestrationLifecycleOperationDto>> GetOrchestrationLifecycleOperationsByStatusAsync(
        GetOrchestrationLifecycleOperationsByStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(query.Status)) throw new InvalidOperationStatusException(query.Status.ToString());
            return await _queryHandler.GetOrchestrationLifecycleOperationsByStatusAsync(query, cancellationToken);
        });

    public Task<List<OrchestrationLifecycleOperationDto>> GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusAsync(
        GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(query.Status)) throw new InvalidOperationStatusException(query.Status.ToString());
            return await _queryHandler.GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusAsync(query, cancellationToken);
        });

    public Task<List<OrchestrationLifecycleOperationDto>> GetPendingOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetPendingOrchestrationLifecycleOperationsAsync(cancellationToken));

    public Task<List<OrchestrationLifecycleOperationDto>> GetRunningOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetRunningOrchestrationLifecycleOperationsAsync(cancellationToken));

    public Task<List<OrchestrationLifecycleOperationDto>> GetFailedOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetFailedOrchestrationLifecycleOperationsAsync(cancellationToken));

    public Task<List<OrchestrationLifecycleOperationDto>> GetDeletedOrchestrationLifecycleOperationsAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _queryHandler.GetDeletedOrchestrationLifecycleOperationsAsync(cancellationToken));

    //Orchestration Runtime
    public Task<OrchestrationLifecycleOperationDto> ExecuteOrchestrationRuntimeOperationAsync(
        CreateOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!Enum.IsDefined(command.Type)) throw new InvalidOperationTypeException(command.Type.ToString());
            var lifecycleQuery = new GetOrchestrationLifecycleByIdQuery { OrchestrationId = command.OrchestrationId };
            _ = await _lifecycleQueryHandler.GetOrchestrationLifecycleByIdAsync(lifecycleQuery, cancellationToken) ??
                   throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId);
            var operation = await CreateOrchestrationLifecycleOperationAsync(command, cancellationToken);
            var operationStarted = false;

            try
            {
                operation = await StartOrchestrationLifecycleOperationAsync(new StartOrchestrationLifecycleOperationCommand { Id = operation.Id }, cancellationToken);
                operationStarted = true;
                var orchestrationId = OrchestrationId.Create(command.OrchestrationId);
                var associations = await _orchestrationSystemRepository.GetByOrchestrationIdAsync(orchestrationId, cancellationToken: cancellationToken);
                if (associations.Count == 0) throw new InvalidOperationException($"Orchestration '{command.OrchestrationId}' has no active system associations.");
                if (!Enum.TryParse<SystemLifecycleOperationTypeFilter>(command.Type.ToString(), ignoreCase: true, out var systemOperationType) || !Enum.IsDefined(systemOperationType))
                    throw new InvalidOperationTypeException(command.Type.ToString());
                foreach (var association in associations)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var systemCommand = new CreateSystemLifecycleOperationCommand { SystemId = association.SystemId.Value, Type = systemOperationType };
                    await _systemManagementUseCaseHandler.ExecuteSystemRuntimeOperationAsync(systemCommand, cancellationToken);
                }

                return await CompleteOrchestrationLifecycleOperationAsync(new CompleteOrchestrationLifecycleOperationCommand { Id = operation.Id }, cancellationToken);
            }
            catch (Exception ex)
            {
                if (operationStarted)
                    await FailOrchestrationLifecycleOperationAsync(
                        new FailOrchestrationLifecycleOperationCommand { Id = operation.Id, ErrorMessage = ex.Message }, CancellationToken.None);
                
                await CancelOrchestrationLifecycleOperationAsync(new CancelOrchestrationLifecycleOperationCommand { Id = operation.Id }, CancellationToken.None);
                throw;
            }
        });
}
