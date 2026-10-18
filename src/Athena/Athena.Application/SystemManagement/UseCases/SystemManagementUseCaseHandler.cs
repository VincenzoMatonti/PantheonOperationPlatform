using Athena.Application.Common.Services;
using Athena.Application.SystemManagement.Commands;
using Athena.Application.SystemManagement.Dtos;
using Athena.Application.SystemManagement.Exceptions;
using Athena.Application.SystemManagement.Queries;

namespace Athena.Application.SystemManagement.UseCases;

public class SystemManagementUseCaseHandler(
    SystemManagementCommandHandler systemCommandHandler,
    SystemManagementQueryHandler systemQueryHandler,
    ApplicationExceptionService exceptionService)
{
    private readonly SystemManagementCommandHandler _systemManagementCommandHandler = systemCommandHandler;
    private readonly SystemManagementQueryHandler _systemManagementQueryHandler = systemQueryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);

    // =================================================================================================================
    // USE CASE COMMANDS - SYSTEM LIFECYCLE
    public Task<SystemLifecycleDto> CreateSystemLifecycleAsync(
        CreateSystemLifecycleCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            var query = new GetSystemLifecycleBySystemIdQuery { SystemId = command.SystemId };
            var existing = await _systemManagementQueryHandler.GetSystemLifecycleBySystemIdAsync(query, cancellationToken);
            if (existing is not null) throw new SystemLifecycleAlreadyExistsException(command.SystemId);
            return await _systemManagementCommandHandler.CreateSystemLifecycleAsync(command, cancellationToken);
        });

    public Task<SystemLifecycleDto> SetSystemDesiredStateAsync(
        SetSystemDesiredStateCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(command.DesiredState)) throw new InvalidDesiredStateException(command.DesiredState.ToString());

            return await _systemManagementCommandHandler.SetSystemDesiredStateAsync(command, cancellationToken)
                ?? throw new SystemLifecycleNotFoundException(command.SystemId);
        });

    public Task<SystemLifecycleDto> MarkSystemStartingAsync(
        MarkSystemStartingCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.MarkSystemStartingAsync(command, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemRunningAsync(
        MarkSystemRunningCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.MarkSystemRunningAsync(command, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemStoppingAsync(
        MarkSystemStoppingCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.MarkSystemStoppingAsync(command, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemStoppedAsync(
        MarkSystemStoppedCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.MarkSystemStoppedAsync(command, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemFailedAsync(
        MarkSystemFailedCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.MarkSystemFailedAsync(command, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemUnknownAsync(
        MarkSystemUnknownCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.MarkSystemUnknownAsync(command, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> DeleteSystemLifecycleAsync(
        DeleteSystemLifecycleCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.DeleteSystemLifecycleAsync(command, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    // =================================================================================================================
    // USE CASE COMMANDS - SYSTEM LIFECYCLE OPERATIONS
    public Task<SystemLifecycleOperationDto> CreateSystemLifecycleOperationAsync(
        CreateSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(command.Type)) throw new InvalidOperationTypeException(command.Type.ToString());
            var lifecycleQuery = new GetSystemLifecycleBySystemIdQuery { SystemId = command.SystemId };
            _ = await _systemManagementQueryHandler.GetSystemLifecycleBySystemIdAsync(lifecycleQuery, cancellationToken)
                ?? throw new SystemLifecycleNotFoundException(command.SystemId);

            return await _systemManagementCommandHandler.CreateSystemLifecycleOperationAsync(command, cancellationToken)
                ?? throw new SystemLifecycleNotFoundException(command.SystemId);
        });

    public Task<SystemLifecycleOperationDto> StartSystemLifecycleOperationAsync(
        StartSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.StartSystemLifecycleOperationAsync(command, cancellationToken)
            ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    public Task<SystemLifecycleOperationDto> CompleteSystemLifecycleOperationAsync(
        CompleteSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.CompleteSystemLifecycleOperationAsync(command, cancellationToken)
            ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    public Task<SystemLifecycleOperationDto> FailSystemLifecycleOperationAsync(
        FailSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.FailSystemLifecycleOperationAsync(command, cancellationToken)
            ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    public Task<SystemLifecycleOperationDto> CancelSystemLifecycleOperationAsync(
        CancelSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.CancelSystemLifecycleOperationAsync(command, cancellationToken)
            ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    public Task<SystemLifecycleOperationDto> DeleteSystemLifecycleOperationAsync(
        DeleteSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementCommandHandler.DeleteSystemLifecycleOperationAsync(command, cancellationToken)
            ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    // =================================================================================================================
    // USE CASE QUERIES - SYSTEM LIFECYCLE
    public Task<SystemLifecycleDto> GetSystemLifecycleBySystemIdAsync(
        GetSystemLifecycleBySystemIdQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementQueryHandler.GetSystemLifecycleBySystemIdAsync(query, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(query.SystemId));

    public Task<List<SystemLifecycleDto>> GetAllSystemLifecyclesAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _systemManagementQueryHandler.GetAllSystemLifecyclesAsync(cancellationToken));

    public Task<List<SystemLifecycleDto>> GetSystemLifecyclesByDesiredStateAsync(
        GetSystemLifecyclesByDesiredStateQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(query.DesiredState)) throw new InvalidDesiredStateException(query.DesiredState.ToString());
            return await _systemManagementQueryHandler.GetSystemLifecyclesByDesiredStateAsync(query, cancellationToken);
        });

    public Task<List<SystemLifecycleDto>> GetSystemLifecyclesByOperationalStatusAsync(
        GetSystemLifecyclesByOperationalStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(query.OperationalStatus)) throw new InvalidOperationalStatusException(query.OperationalStatus.ToString());
            return await _systemManagementQueryHandler.GetSystemLifecyclesByOperationalStatusAsync(query, cancellationToken);
        });

    public Task<List<SystemLifecycleDto>> GetSystemLifecyclesByDeletionStatusAsync(
        GetSystemLifecyclesByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _systemManagementQueryHandler.GetSystemLifecyclesByDeletionStatusAsync(query, cancellationToken));

    public Task<List<SystemLifecycleDto>> GetDeletedSystemLifecyclesAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _systemManagementQueryHandler.GetDeletedSystemLifecyclesAsync(cancellationToken));

    // =================================================================================================================
    // USE CASE QUERIES - SYSTEM LIFECYCLE OPERATIONS
    public Task<SystemLifecycleOperationDto> GetSystemLifecycleOperationByIdAsync(
        GetSystemLifecycleOperationByIdQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
            await _systemManagementQueryHandler.GetSystemLifecycleOperationByIdAsync(query, cancellationToken)
            ?? throw new SystemLifecycleOperationNotFoundException(query.OperationId));

    public Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsBySystemIdAsync(
        GetSystemLifecycleOperationsBySystemIdQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _systemManagementQueryHandler.GetSystemLifecycleOperationsBySystemIdAsync(query, cancellationToken));

    public Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsByStatusAsync(
        GetSystemLifecycleOperationsByStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(query.Status)) throw new InvalidOperationStatusException(query.Status.ToString());
            return await _systemManagementQueryHandler.GetSystemLifecycleOperationsByStatusAsync(query, cancellationToken);
        });

    public Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsBySystemIdAndStatusAsync(
        GetSystemLifecycleOperationsBySystemIdAndStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(async () =>
        {
            if (!Enum.IsDefined(query.Status)) throw new InvalidOperationStatusException(query.Status.ToString());
            return await _systemManagementQueryHandler.GetSystemLifecycleOperationsBySystemIdAndStatusAsync(query, cancellationToken);
        });

    public Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsByDeletionStatusAsync(
        GetSystemLifecycleOperationsByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _systemManagementQueryHandler.GetSystemLifecycleOperationsByDeletionStatusAsync(query, cancellationToken));

    public Task<List<SystemLifecycleOperationDto>> GetDeletedSystemLifecycleOperationsAsync(CancellationToken cancellationToken = default) =>
        ExecuteAsync(() => _systemManagementQueryHandler.GetDeletedSystemLifecycleOperationsAsync(cancellationToken));
}