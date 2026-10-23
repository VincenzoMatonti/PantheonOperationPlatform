using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Services;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Enums;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.SystemManagement.Commands;
using Athena.Application.SystemManagement.Enums;
using Athena.Application.SystemManagement.UseCases;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationRegistry.ValueObjects;

namespace Athena.Application.OrchestrationManagement.UseCases;

public class OrchestrationRuntimeUseCaseHandler(
    ApplicationExceptionService exceptionService,
    OrchestrationLifecycleQueryHandler lifecycleQueryHandler,
    SystemManagementUseCaseHandler systemManagementUseCaseHandler,
    OrchestrationLifecycleUseCaseHandler orchestrationLifecycleUseCaseHandler,
    OrchestrationLifecycleOperationUseCaseHandler orchestrationLifecycleOperationUseCaseHandler,
    IOrchestrationSystemRepository orchestrationSystemRepository,
    IRuntimeDiscoveryService runtimeDiscoveryService)
{

    private readonly ApplicationExceptionService _exceptionService = exceptionService;
    private readonly OrchestrationLifecycleQueryHandler _lifecycleQueryHandler = lifecycleQueryHandler;
    private readonly SystemManagementUseCaseHandler _systemManagementUseCaseHandler = systemManagementUseCaseHandler;
    private readonly OrchestrationLifecycleUseCaseHandler _orchestrationLifecycleUseCaseHandler = orchestrationLifecycleUseCaseHandler;
    private readonly OrchestrationLifecycleOperationUseCaseHandler _orchestrationLifecycleOperationUseCaseHandler = orchestrationLifecycleOperationUseCaseHandler;
    private readonly IOrchestrationSystemRepository _orchestrationSystemRepository = orchestrationSystemRepository;
    private readonly IRuntimeDiscoveryService _runtimeDiscoveryService = runtimeDiscoveryService;  

    //Orchestration Runtime
    public Task<OrchestrationLifecycleOperationDto> ExecuteOrchestrationRuntimeOperationAsync(
        CreateOrchestrationLifecycleOperationCommand command, CancellationToken cancellationToken = default)
    {
        OrchestrationLifecycleOperationDto? operation = null;
        var operationStarted = false;
        return _exceptionService.ExecuteWithRecoveryAsync(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!Enum.IsDefined(command.Type)) throw new InvalidOperationTypeException(command.Type.ToString());
                var lifecycleQuery = new GetOrchestrationLifecycleByIdQuery { OrchestrationId = command.OrchestrationId };
                _ = await _lifecycleQueryHandler.GetOrchestrationLifecycleByIdAsync(lifecycleQuery, cancellationToken) ??
                    throw new OrchestrationLifecycleNotFoundException(command.OrchestrationId);
                operation = await _orchestrationLifecycleOperationUseCaseHandler.CreateOrchestrationLifecycleOperationAsync(command, cancellationToken);
                operation = await _orchestrationLifecycleOperationUseCaseHandler.StartOrchestrationLifecycleOperationAsync(new StartOrchestrationLifecycleOperationCommand { Id = operation.Id }, cancellationToken);
                operationStarted = true;
                if (command.Type is OrchestrationLifecycleOperationType.Start or OrchestrationLifecycleOperationType.Restart)
                {
                    await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationStartingAsync(new MarkOrchestrationStartingCommand { OrchestrationId = command.OrchestrationId }, cancellationToken);
                }
                else if (command.Type == OrchestrationLifecycleOperationType.Stop)
                {
                    await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationStoppingAsync(new MarkOrchestrationStoppingCommand { OrchestrationId = command.OrchestrationId }, cancellationToken);
                }

                var orchestrationId = OrchestrationId.Create(command.OrchestrationId);
                var associations = await _orchestrationSystemRepository.GetByOrchestrationIdAsync(orchestrationId, cancellationToken: cancellationToken);
                if (associations.Count == 0) throw new InvalidOperationException($"Orchestration '{command.OrchestrationId}' has no active system associations.");
                if (!Enum.TryParse<SystemLifecycleOperationTypeFilter>(command.Type.ToString(), ignoreCase: true, out var systemOperationType) || !Enum.IsDefined(systemOperationType))
                    throw new InvalidOperationTypeException(command.Type.ToString());

                await _runtimeDiscoveryService.DiscoverAndSynchronizeAsync(cancellationToken);
                foreach (var association in associations)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var systemCommand = new CreateSystemLifecycleOperationCommand { SystemId = association.SystemId.Value, Type = systemOperationType };
                    await _systemManagementUseCaseHandler.ExecuteSystemRuntimeOperationAsync(systemCommand, cancellationToken);
                }

                if (command.Type == OrchestrationLifecycleOperationType.Stop)
                {
                    await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationStoppedAsync(
                            new MarkOrchestrationStoppedCommand { OrchestrationId = command.OrchestrationId }, cancellationToken);
                }
                else
                {
                    await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationRunningAsync(
                            new MarkOrchestrationRunningCommand { OrchestrationId = command.OrchestrationId }, cancellationToken);
                }

                return await _orchestrationLifecycleOperationUseCaseHandler.CompleteOrchestrationLifecycleOperationAsync(
                    new CompleteOrchestrationLifecycleOperationCommand { Id = operation.Id }, cancellationToken);
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
    }

}
