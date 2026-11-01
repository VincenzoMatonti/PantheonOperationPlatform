using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Services;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Enums;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Interfaces;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.SystemManagement.Contracts;
using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;

namespace Athena.Application.OrchestrationManagement.UseCases;

public class OrchestrationProcessUseCaseHandler(
    ApplicationExceptionService exceptionService,
    IOrchestrationLifecycleQueries lifecycleQueryHandler,
    IOrchestrationLifecycleCommands orchestrationLifecycleUseCaseHandler,
    IOrchestrationLifecycleOperations orchestrationLifecycleOperationUseCaseHandler,
    IOrchestrationStatusSynchronizer orchestrationStatusSynchronizationService,
    IOrchestrationSystemInstanceRepository orchestrationSystemRepository,
    ISystemInstanceProcessCoordinator processManagementService)
{

    private readonly ApplicationExceptionService _exceptionService = exceptionService;
    private readonly IOrchestrationLifecycleQueries _lifecycleQueryHandler = lifecycleQueryHandler;
    private readonly IOrchestrationLifecycleCommands _orchestrationLifecycleUseCaseHandler = orchestrationLifecycleUseCaseHandler;
    private readonly IOrchestrationLifecycleOperations _orchestrationLifecycleOperationUseCaseHandler = orchestrationLifecycleOperationUseCaseHandler;
    private readonly IOrchestrationStatusSynchronizer _orchestrationStatusSynchronizationService = orchestrationStatusSynchronizationService;
    private readonly IOrchestrationSystemInstanceRepository _orchestrationSystemRepository = orchestrationSystemRepository;
    private readonly ISystemInstanceProcessCoordinator _processManagementService = processManagementService;

    public async Task<OrchestrationLifecycleOperationDto> ExecuteOrchestrationOperationAsync(CreateOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default)
    {
        OrchestrationLifecycleOperationDto? operation = null;
        var operationStarted = false;
        IReadOnlyCollection<OrchestrationSystemInstance> associations = [];
        var completedOperation = await _exceptionService.ExecuteWithRecoveryAsync(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!Enum.IsDefined(command.Type)) throw new InvalidOperationTypeException(command.Type.ToString());
            var orchestrationId = OrchestrationId.Create(command.OrchestrationId);
            associations = (await _orchestrationSystemRepository.GetByOrchestrationIdAsync(
                    orchestrationId, cancellationToken: cancellationToken))
                .OrderBy(association => association.SystemInstanceId.Value)
                .ToArray();
            if (associations.Count == 0)
                throw new InvalidOperationException($"Orchestration '{command.OrchestrationId}' has no active SystemInstance associations.");

            foreach (var association in associations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await _processManagementService.EnsureOperableAsync(association.SystemInstanceId.Value, cancellationToken);
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

            var failures = new Dictionary<Guid, Exception>();
            var successfulInstanceIds = new List<Guid>(associations.Count);
            foreach (var association in associations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    switch (command.Type)
                    {
                        case OrchestrationLifecycleOperationType.Start:
                            await _processManagementService.StartAsync(association.SystemInstanceId.Value, cancellationToken);
                            break;
                        case OrchestrationLifecycleOperationType.Stop:
                            await _processManagementService.StopAsync(association.SystemInstanceId.Value, cancellationToken);
                            break;
                        case OrchestrationLifecycleOperationType.Restart:
                            await _processManagementService.RestartAsync(association.SystemInstanceId.Value, cancellationToken);
                            break;
                        default:
                            throw new InvalidOperationTypeException(command.Type.ToString());
                    }

                    successfulInstanceIds.Add(association.SystemInstanceId.Value);
                }
                catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
                {
                    failures.Add(association.SystemInstanceId.Value, exception);
                }
            }

            if (failures.Count > 0)
                throw new OrchestrationSystemInstanceOperationsFailedException(command.OrchestrationId, failures);

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
                                associations.Select(association => association.SystemInstanceId.Value), CancellationToken.None, excludedOrchestrationId: command.OrchestrationId),
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
