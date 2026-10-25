using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.DTOs;
using Athena.Application.OrchestrationManagement.Enums;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Queries;

namespace Athena.Application.OrchestrationManagement.UseCases;

public class OrchestrationDesiredStateUseCaseHandler(OrchestrationLifecycleUseCaseHandler lifecycleHandler, OrchestrationRuntimeUseCaseHandler runtimeHandler)
{
    private readonly OrchestrationLifecycleUseCaseHandler _lifecycleHandler = lifecycleHandler;
    private readonly OrchestrationRuntimeUseCaseHandler _runtimeHandler = runtimeHandler;

    public async Task<OrchestrationLifecycleDto> SetOrchestrationDesiredStateAsync(SetOrchestrationDesiredStateCommand command, CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(command.DesiredState)) throw new InvalidDesiredStateException(command.DesiredState.ToString());
        switch (command.DesiredState)
        {
            case OrchestrationDesiredState.Running:
                await _runtimeHandler.ExecuteOrchestrationRuntimeOperationAsync(
                    new CreateOrchestrationLifecycleOperationCommand
                    {
                        Id = Guid.NewGuid(),
                        OrchestrationId = command.OrchestrationId,
                        Type = OrchestrationLifecycleOperationType.Start
                    }, cancellationToken);
                break;

            case OrchestrationDesiredState.Stopped:
                await _runtimeHandler.ExecuteOrchestrationRuntimeOperationAsync(
                    new CreateOrchestrationLifecycleOperationCommand
                    {
                        Id = Guid.NewGuid(),
                        OrchestrationId = command.OrchestrationId,
                        Type = OrchestrationLifecycleOperationType.Stop
                    }, cancellationToken);
                break;

            case OrchestrationDesiredState.Unknown: return await _lifecycleHandler.SetOrchestrationDesiredStateAsync(command, cancellationToken);

            default:
                throw new InvalidDesiredStateException(command.DesiredState.ToString());
        }

        return await _lifecycleHandler.GetOrchestrationLifecycleByIdAsync(new GetOrchestrationLifecycleByIdQuery { OrchestrationId = command.OrchestrationId }, cancellationToken);
    }
}
