using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Services;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Enums;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Interfaces;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.SystemManagement.Contracts;

namespace Athena.Application.OrchestrationManagement.UseCases;

public class OrchestrationProcessUseCaseHandler(
    ApplicationExceptionService exceptionService,
    IOrchestrationLifecycleQueries lifecycleQueryHandler,
    IOrchestrationLifecycleCommands orchestrationLifecycleUseCaseHandler,
    IOrchestrationLifecycleOperations orchestrationLifecycleOperationUseCaseHandler,
    IOrchestrationStatusSynchronizer orchestrationStatusSynchronizationService,
    IOrchestrationSystemInstanceQueries orchestrationSystemQueries,
    ISystemInstanceProcessCoordinator processManagementService,
    IOrchestrationResourceCoordinator resourceCoordinator)
{

    private readonly ApplicationExceptionService _exceptionService = exceptionService;
    private readonly IOrchestrationLifecycleQueries _lifecycleQueryHandler = lifecycleQueryHandler;
    private readonly IOrchestrationLifecycleCommands _orchestrationLifecycleUseCaseHandler = orchestrationLifecycleUseCaseHandler;
    private readonly IOrchestrationLifecycleOperations _orchestrationLifecycleOperationUseCaseHandler = orchestrationLifecycleOperationUseCaseHandler;
    private readonly IOrchestrationStatusSynchronizer _orchestrationStatusSynchronizationService = orchestrationStatusSynchronizationService;
    private readonly IOrchestrationSystemInstanceQueries _orchestrationSystemQueries = orchestrationSystemQueries;
    private readonly ISystemInstanceProcessCoordinator _processManagementService = processManagementService;
    private readonly IOrchestrationResourceCoordinator _resourceCoordinator = resourceCoordinator;

    public async Task<OrchestrationLifecycleOperationDto> ExecuteOrchestrationOperationAsync(CreateOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default)
    {
        OrchestrationLifecycleOperationDto? operation = null;
        var operationStarted = false;
        IReadOnlyList<OrchestrationSystemInstanceDto> associations = [];
        var completedOperation = await _exceptionService.ExecuteWithRecoveryAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!Enum.IsDefined(command.Type)) throw new InvalidOperationTypeException(command.Type.ToString());
            associations = await _orchestrationSystemQueries.GetByOrchestrationIdAsync(
                new GetOrchestrationSystemInstancesByOrchestrationIdQuery(command.OrchestrationId),
                cancellationToken);
            if (associations.Count == 0)
                throw new InvalidOperationException($"Orchestration '{command.OrchestrationId}' has no active SystemInstance associations.");

            foreach (var association in associations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _processManagementService.EnsureOperableAsync(association.SystemInstanceId, cancellationToken);
            }

            var lifecycleQuery = new GetOrchestrationLifecycleByIdQuery { OrchestrationId = command.OrchestrationId };
            var lifecycle = await _lifecycleQueryHandler.GetOrchestrationLifecycleByIdAsync(lifecycleQuery, cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId);
            var desiredState = command.Type switch
            {
                OrchestrationLifecycleOperationType.Start => OrchestrationDesiredState.Running,
                OrchestrationLifecycleOperationType.Stop => OrchestrationDesiredState.Stopped,
                OrchestrationLifecycleOperationType.Restart => OrchestrationDesiredState.Running,
                _ => OrchestrationDesiredState.Unknown
            };

            if (lifecycle.DesiredState != desiredState)
                await _orchestrationLifecycleUseCaseHandler.SetOrchestrationDesiredStateAsync(
                    new SetOrchestrationDesiredStateCommand { OrchestrationId = command.OrchestrationId, DesiredState = desiredState }, cancellationToken);

            operation = await _orchestrationLifecycleOperationUseCaseHandler.CreateOrchestrationLifecycleOperationAsync(command, cancellationToken);
            operation = await _orchestrationLifecycleOperationUseCaseHandler.StartOrchestrationLifecycleOperationAsync(new StartOrchestrationLifecycleOperationCommand { Id = operation.Id }, cancellationToken);
            operationStarted = true;
            if (command.Type is OrchestrationLifecycleOperationType.Start or OrchestrationLifecycleOperationType.Restart)
                await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationStartingAsync(new MarkOrchestrationStartingCommand { OrchestrationId = command.OrchestrationId }, cancellationToken);
            else if (command.Type == OrchestrationLifecycleOperationType.Stop)
                await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationStoppingAsync(new MarkOrchestrationStoppingCommand { OrchestrationId = command.OrchestrationId }, cancellationToken);

            var successfulInstanceIds = await _resourceCoordinator.ExecuteAsync(
                command.OrchestrationId,
                associations.Select(association => association.SystemInstanceId).ToArray(),
                command.Type.ToString(),
                cancellationToken);

            operation = await _orchestrationLifecycleOperationUseCaseHandler.CompleteOrchestrationLifecycleOperationAsync(
                new CompleteOrchestrationLifecycleOperationCommand { Id = operation.Id }, cancellationToken);
            return (Operation: operation, SuccessfulInstanceIds: successfulInstanceIds);
        },
        async originalException =>
        {
            if (operation is null) return;
            if (operationStarted)
            {
                await ApplicationExceptionService.TryExecuteCleanupAsync(() => _orchestrationLifecycleUseCaseHandler.MarkOrchestrationFailedAsync(
                        new MarkOrchestrationFailedCommand { OrchestrationId = command.OrchestrationId }, CancellationToken.None),
                        $"Failed to mark orchestration '{command.OrchestrationId}' as Failed.");

                await ApplicationExceptionService.TryExecuteCleanupAsync(async () =>
                {
                    _ = await _orchestrationLifecycleOperationUseCaseHandler.FailOrchestrationLifecycleOperationAsync(
                        new FailOrchestrationLifecycleOperationCommand { Id = operation.Id, ErrorMessage = originalException.ToString() }, CancellationToken.None);

                }, $"Failed to mark orchestration operation '{operation.Id}' as Failed.");

                if (associations.Count > 0)
                {
                    await ApplicationExceptionService.TryExecuteCleanupAsync(() => _orchestrationStatusSynchronizationService.SynchronizeForSystemInstancesAsync(
                                associations.Select(association => association.SystemInstanceId), CancellationToken.None, excludedOrchestrationId: command.OrchestrationId),
                                $"Failed to synchronize related orchestrations after orchestration '{command.OrchestrationId}' failed.");
                }
            }
            else
            {
                await ApplicationExceptionService.TryExecuteCleanupAsync(async () =>
                {
                    _ = await _orchestrationLifecycleOperationUseCaseHandler.CancelOrchestrationLifecycleOperationAsync(
                        new CancelOrchestrationLifecycleOperationCommand { Id = operation.Id }, CancellationToken.None);

                }, $"Failed to cancel orchestration operation '{operation.Id}'.");
            }
        });

        await _orchestrationStatusSynchronizationService.SynchronizeForSystemInstancesAsync(
            completedOperation.SuccessfulInstanceIds, cancellationToken);
        return completedOperation.Operation;
    }
}
