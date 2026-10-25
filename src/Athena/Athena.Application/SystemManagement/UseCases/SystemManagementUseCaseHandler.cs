using Athena.Application.Common.Dtos;
using Athena.Application.Common.Services;
using Athena.Application.SystemManagement.Commands;
using Athena.Application.SystemManagement.Dtos;
using Athena.Application.SystemManagement.Enums;
using Athena.Application.SystemManagement.Exceptions;
using Athena.Application.SystemManagement.Queries;

namespace Athena.Application.SystemManagement.UseCases;

public class SystemManagementUseCaseHandler(
    SystemManagementCommandHandler systemCommandHandler,
    SystemManagementQueryHandler systemQueryHandler,
    ApplicationExceptionService exceptionService,
    RuntimeApplicationService runtimeApplicationService,
    OrchestrationStatusSynchronizationService orchestrationStatusSynchronizationService)
{
    private readonly SystemManagementCommandHandler _systemManagementCommandHandler = systemCommandHandler;
    private readonly SystemManagementQueryHandler _systemManagementQueryHandler = systemQueryHandler;
    private readonly ApplicationExceptionService _exceptionService = exceptionService;
    private readonly RuntimeApplicationService _runtimeApplicationService = runtimeApplicationService;
    private readonly OrchestrationStatusSynchronizationService _orchestrationStatusSynchronizationService = orchestrationStatusSynchronizationService;

    private Task<T> ExecuteAsync<T>(Func<Task<T>> operation) => _exceptionService.ExecuteAsync(operation);

    // =================================================================================================================
    // USE CASE COMMANDS - SYSTEM LIFECYCLE
    public Task<SystemLifecycleDto> CreateSystemLifecycleAsync(CreateSystemLifecycleCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        var query = new GetSystemLifecycleBySystemIdQuery { SystemId = command.SystemId };
        var existing = await _systemManagementQueryHandler.GetSystemLifecycleBySystemIdAsync(query, cancellationToken);
        if (existing is not null) throw new SystemLifecycleAlreadyExistsException(command.SystemId);
        return await _systemManagementCommandHandler.CreateSystemLifecycleAsync(command, cancellationToken);
    });

    public Task<SystemLifecycleDto> SetSystemDesiredStateAsync(SetSystemDesiredStateCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(command.DesiredState)) throw new InvalidDesiredStateException(command.DesiredState.ToString());
        switch (command.DesiredState)
        {
            case SystemDesiredStateFilter.Running:
                await ExecuteSystemRuntimeOperationAsync(new CreateSystemLifecycleOperationCommand { SystemId = command.SystemId, Type = SystemLifecycleOperationTypeFilter.Start }, cancellationToken);
                break;

            case SystemDesiredStateFilter.Stopped:
                await ExecuteSystemRuntimeOperationAsync(new CreateSystemLifecycleOperationCommand { SystemId = command.SystemId, Type = SystemLifecycleOperationTypeFilter.Stop }, cancellationToken);
                break;

            case SystemDesiredStateFilter.Unknown:
                return await _systemManagementCommandHandler.SetSystemDesiredStateAsync(command, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId);

            default:
                throw new InvalidDesiredStateException(command.DesiredState.ToString());
        }

        return await _systemManagementQueryHandler.GetSystemLifecycleBySystemIdAsync(
           new GetSystemLifecycleBySystemIdQuery { SystemId = command.SystemId }, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId);
    });

    public Task<SystemLifecycleDto> MarkSystemStartingAsync(MarkSystemStartingCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.MarkSystemStartingAsync(command, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemRunningAsync(MarkSystemRunningCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.MarkSystemRunningAsync(command, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemStoppingAsync(MarkSystemStoppingCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.MarkSystemStoppingAsync(command, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemStoppedAsync(MarkSystemStoppedCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.MarkSystemStoppedAsync(command, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemFailedAsync(MarkSystemFailedCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.MarkSystemFailedAsync(command, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> MarkSystemUnknownAsync(MarkSystemUnknownCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.MarkSystemUnknownAsync(command, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    public Task<SystemLifecycleDto> DeleteSystemLifecycleAsync(DeleteSystemLifecycleCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.DeleteSystemLifecycleAsync(command, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId));

    // =================================================================================================================
    // USE CASE COMMANDS - SYSTEM LIFECYCLE OPERATIONS
    public Task<SystemLifecycleOperationDto> CreateSystemLifecycleOperationAsync(CreateSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(command.Type)) throw new InvalidOperationTypeException(command.Type.ToString());
        var lifecycleQuery = new GetSystemLifecycleBySystemIdQuery { SystemId = command.SystemId };
        _ = await _systemManagementQueryHandler.GetSystemLifecycleBySystemIdAsync(lifecycleQuery, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(command.SystemId);

        return await _systemManagementCommandHandler.CreateSystemLifecycleOperationAsync(command, cancellationToken)
            ?? throw new SystemLifecycleNotFoundException(command.SystemId);
    });

    public Task<SystemLifecycleOperationDto> StartSystemLifecycleOperationAsync(StartSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.StartSystemLifecycleOperationAsync(command, cancellationToken) ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    public Task<SystemLifecycleOperationDto> CompleteSystemLifecycleOperationAsync(CompleteSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.CompleteSystemLifecycleOperationAsync(command, cancellationToken) ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    public Task<SystemLifecycleOperationDto> FailSystemLifecycleOperationAsync(FailSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.FailSystemLifecycleOperationAsync(command, cancellationToken) ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    public Task<SystemLifecycleOperationDto> CancelSystemLifecycleOperationAsync(CancelSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.CancelSystemLifecycleOperationAsync(command, cancellationToken) ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    public Task<SystemLifecycleOperationDto> DeleteSystemLifecycleOperationAsync(DeleteSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementCommandHandler.DeleteSystemLifecycleOperationAsync(command, cancellationToken) ?? throw new SystemLifecycleOperationNotFoundException(command.OperationId));

    // =================================================================================================================
    // USE CASE QUERIES - SYSTEM LIFECYCLE
    public Task<SystemLifecycleDto> GetSystemLifecycleBySystemIdAsync(GetSystemLifecycleBySystemIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementQueryHandler.GetSystemLifecycleBySystemIdAsync(query, cancellationToken) ?? throw new SystemLifecycleNotFoundException(query.SystemId));

    public Task<List<SystemLifecycleDto>> GetAllSystemLifecyclesAsync(CancellationToken cancellationToken = default) => ExecuteAsync(() =>
        _systemManagementQueryHandler.GetAllSystemLifecyclesAsync(cancellationToken));

    public Task<List<SystemLifecycleDto>> GetSystemLifecyclesByDesiredStateAsync(GetSystemLifecyclesByDesiredStateQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.DesiredState)) throw new InvalidDesiredStateException(query.DesiredState.ToString());
        return await _systemManagementQueryHandler.GetSystemLifecyclesByDesiredStateAsync(query, cancellationToken);
    });

    public Task<List<SystemLifecycleDto>> GetSystemLifecyclesByOperationalStatusAsync(GetSystemLifecyclesByOperationalStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.OperationalStatus)) throw new InvalidOperationalStatusException(query.OperationalStatus.ToString());
        return await _systemManagementQueryHandler.GetSystemLifecyclesByOperationalStatusAsync(query, cancellationToken);
    });

    public Task<List<SystemLifecycleDto>> GetSystemLifecyclesByDeletionStatusAsync(GetSystemLifecyclesByDeletionStatusQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(() =>
        _systemManagementQueryHandler.GetSystemLifecyclesByDeletionStatusAsync(query, cancellationToken));

    public Task<List<SystemLifecycleDto>> GetDeletedSystemLifecyclesAsync(CancellationToken cancellationToken = default) => ExecuteAsync(() =>
        _systemManagementQueryHandler.GetDeletedSystemLifecyclesAsync(cancellationToken));

    // =================================================================================================================
    // USE CASE QUERIES - SYSTEM LIFECYCLE OPERATIONS
    public Task<SystemLifecycleOperationDto> GetSystemLifecycleOperationByIdAsync(GetSystemLifecycleOperationByIdQuery query, CancellationToken cancellationToken = default) => ExecuteAsync(async () =>
        await _systemManagementQueryHandler.GetSystemLifecycleOperationByIdAsync(query, cancellationToken) ?? throw new SystemLifecycleOperationNotFoundException(query.OperationId));

    public Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsBySystemIdAsync(GetSystemLifecycleOperationsBySystemIdQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _systemManagementQueryHandler.GetSystemLifecycleOperationsBySystemIdAsync(query, cancellationToken));

    public Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsByStatusAsync(GetSystemLifecycleOperationsByStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.Status)) throw new InvalidOperationStatusException(query.Status.ToString());
        return await _systemManagementQueryHandler.GetSystemLifecycleOperationsByStatusAsync(query, cancellationToken);
    });

    public Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsBySystemIdAndStatusAsync(GetSystemLifecycleOperationsBySystemIdAndStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(async () =>
    {
        if (!Enum.IsDefined(query.Status)) throw new InvalidOperationStatusException(query.Status.ToString());
        return await _systemManagementQueryHandler.GetSystemLifecycleOperationsBySystemIdAndStatusAsync(query, cancellationToken);
    });

    public Task<List<SystemLifecycleOperationDto>> GetSystemLifecycleOperationsByDeletionStatusAsync(GetSystemLifecycleOperationsByDeletionStatusQuery query, CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _systemManagementQueryHandler.GetSystemLifecycleOperationsByDeletionStatusAsync(query, cancellationToken));

    public Task<List<SystemLifecycleOperationDto>> GetDeletedSystemLifecycleOperationsAsync(CancellationToken cancellationToken = default) =>
    ExecuteAsync(() => _systemManagementQueryHandler.GetDeletedSystemLifecycleOperationsAsync(cancellationToken));

    // =================================================================================================================
    // SYSTEM RUNTIME OPERATION
    public Task<SystemLifecycleOperationDto> ExecuteSystemRuntimeOperationAsync(
        CreateSystemLifecycleOperationCommand command, CancellationToken cancellationToken = default, bool synchronizeRelatedOrchestrations = true)
    {
        SystemLifecycleOperationDto? operation = null;
        var operationStarted = false;
        return _exceptionService.ExecuteWithRecoveryAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!Enum.IsDefined(command.Type)) throw new InvalidOperationTypeException(command.Type.ToString());
            var lifecycleQuery = new GetSystemLifecycleBySystemIdQuery { SystemId = command.SystemId };
            var lifecycle = await _systemManagementQueryHandler.GetSystemLifecycleBySystemIdAsync(lifecycleQuery, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId);
            var desiredState = command.Type switch
            {
                SystemLifecycleOperationTypeFilter.Start => SystemDesiredStateFilter.Running,
                SystemLifecycleOperationTypeFilter.Stop => SystemDesiredStateFilter.Stopped,
                SystemLifecycleOperationTypeFilter.Restart => SystemDesiredStateFilter.Running,
                _ => SystemDesiredStateFilter.Unknown
            };

            if (!string.Equals(lifecycle.DesiredState, desiredState.ToString(), StringComparison.Ordinal))
                _ = await _systemManagementCommandHandler.SetSystemDesiredStateAsync(
                    new SetSystemDesiredStateCommand { SystemId = command.SystemId, DesiredState = desiredState }, cancellationToken) ?? throw new SystemLifecycleNotFoundException(command.SystemId);

            operation = await _systemManagementCommandHandler.CreateSystemLifecycleOperationAsync(command, cancellationToken) ??
                throw new SystemLifecycleNotFoundException(command.SystemId);

            operation = await _systemManagementCommandHandler.StartSystemLifecycleOperationAsync(
                    new StartSystemLifecycleOperationCommand { OperationId = operation.Id }, cancellationToken) ??
                        throw new SystemLifecycleOperationNotFoundException(operation.Id);

            operationStarted = true;
            var instances = await _runtimeApplicationService.GetBySystemIdAsync(command.SystemId);
            switch (command.Type)
            {
                case SystemLifecycleOperationTypeFilter.Start:
                    {
                        if (lifecycle.OperationalStatus == "Running") break;
                        if (lifecycle.OperationalStatus is not ("Unknown" or "Stopped" or "Failed"))
                            throw new InvalidOperationException($"Cannot start system '{command.SystemId}' while its operational status is '{lifecycle.OperationalStatus}'.");

                        EnsureRuntimeInstancesAvailable(instances, command.SystemId);
                        await MarkSystemStartingAsync(new MarkSystemStartingCommand { SystemId = command.SystemId }, cancellationToken);
                        await ExecuteOnAllInstancesAsync(instances, static (runtime, id) => runtime.StartAsync(id), cancellationToken);
                        await MarkSystemRunningAsync(new MarkSystemRunningCommand { SystemId = command.SystemId }, cancellationToken);
                        break;
                    }

                case SystemLifecycleOperationTypeFilter.Stop:
                    {
                        if (lifecycle.OperationalStatus == "Stopped") break;
                        if (lifecycle.OperationalStatus != "Running")
                            throw new InvalidOperationException($"Cannot stop system '{command.SystemId}' while its operational status is '{lifecycle.OperationalStatus}'.");

                        EnsureRuntimeInstancesAvailable(instances, command.SystemId);
                        await MarkSystemStoppingAsync(new MarkSystemStoppingCommand { SystemId = command.SystemId }, cancellationToken);
                        await ExecuteOnAllInstancesAsync(instances, static (runtime, id) => runtime.StopAsync(id), cancellationToken);
                        await MarkSystemStoppedAsync(new MarkSystemStoppedCommand { SystemId = command.SystemId }, cancellationToken);
                        break;
                    }

                case SystemLifecycleOperationTypeFilter.Restart:
                    {
                        if (lifecycle.OperationalStatus is "Starting" or "Stopping")
                            throw new InvalidOperationException($"Cannot restart system '{command.SystemId}' while its operational status is '{lifecycle.OperationalStatus}'.");

                        EnsureRuntimeInstancesAvailable(instances, command.SystemId);
                        if (lifecycle.OperationalStatus == "Running")
                        {
                            await MarkSystemStoppingAsync(new MarkSystemStoppingCommand { SystemId = command.SystemId }, cancellationToken);
                            await ExecuteOnAllInstancesAsync(instances, static (runtime, id) => runtime.StopAsync(id), cancellationToken);
                            await MarkSystemStoppedAsync(new MarkSystemStoppedCommand { SystemId = command.SystemId }, cancellationToken);
                        }
                        else if (lifecycle.OperationalStatus is not ("Unknown" or "Stopped" or "Failed"))
                            throw new InvalidOperationException($"Cannot restart system '{command.SystemId}' while its operational status is '{lifecycle.OperationalStatus}'.");

                        await MarkSystemStartingAsync(new MarkSystemStartingCommand { SystemId = command.SystemId }, cancellationToken);
                        await ExecuteOnAllInstancesAsync(instances, static (runtime, id) => runtime.StartAsync(id), cancellationToken);
                        await MarkSystemRunningAsync(new MarkSystemRunningCommand { SystemId = command.SystemId }, cancellationToken);
                        break;
                    }

                default:
                    throw new InvalidOperationTypeException(command.Type.ToString());
            }

            var completedOperation = await _systemManagementCommandHandler.CompleteSystemLifecycleOperationAsync(new CompleteSystemLifecycleOperationCommand { OperationId = operation.Id }, cancellationToken) ??
                  throw new SystemLifecycleOperationNotFoundException(operation.Id);
            if (synchronizeRelatedOrchestrations) await _orchestrationStatusSynchronizationService.SynchronizeForSystemsAsync([command.SystemId], cancellationToken);
            return completedOperation;
        },
       async originalException =>
       {
           if (operation is null) return;
           if (operationStarted)
           {
               await ApplicationExceptionService.TryExecuteCleanupAsync(async () =>
               {
                   _ = await _systemManagementCommandHandler.FailSystemLifecycleOperationAsync(
                           new FailSystemLifecycleOperationCommand { OperationId = operation.Id, ErrorMessage = originalException.ToString() },
                           CancellationToken.None);

               }, $"Failed to mark system lifecycle operation '{operation.Id}' as Failed.");

               await ApplicationExceptionService.TryExecuteCleanupAsync(async () =>
               {
                   await MarkSystemFailedAsync(new MarkSystemFailedCommand { SystemId = command.SystemId }, CancellationToken.None);
                   if (synchronizeRelatedOrchestrations) await _orchestrationStatusSynchronizationService.SynchronizeForSystemsAsync([command.SystemId], CancellationToken.None);

               }, $"Failed to mark system '{command.SystemId}' as Failed.");
           }
           else
           {
               await ApplicationExceptionService.TryExecuteCleanupAsync(async () =>
               {
                   _ = await _systemManagementCommandHandler.CancelSystemLifecycleOperationAsync(
                           new CancelSystemLifecycleOperationCommand { OperationId = operation.Id }, CancellationToken.None);

               }, $"Failed to cancel system lifecycle operation '{operation.Id}'.");
           }
       });
    }

    private async Task ExecuteOnAllInstancesAsync(IEnumerable<SystemRuntimeInstanceDto> instances, Func<RuntimeApplicationService, Guid, Task> execute, CancellationToken cancellationToken)
    {
        foreach (var instance in instances)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await execute(_runtimeApplicationService, instance.Id);
        }
    }


    private static void EnsureRuntimeInstancesAvailable(IReadOnlyCollection<SystemRuntimeInstanceDto> instances, Guid systemId)
    {
        if (instances.Count == 0) throw new InvalidOperationException($"No runtime instances are registered for system '{systemId}'.");
    }

}