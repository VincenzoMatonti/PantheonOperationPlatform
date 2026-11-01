using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.Enums;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Interfaces;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.OrchestrationManagement.UseCases;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Application.Common.Services;

public sealed class OrchestrationStatusSynchronizationService(
    IOrchestrationSystemInstanceRepository associationRepository,
    IOrchestrationResourceCoordinator resourceCoordinator,
    OrchestrationLifecycleQueryHandler orchestrationLifecycleQueryHandler,
    OrchestrationLifecycleUseCaseHandler orchestrationLifecycleUseCaseHandler) : IOrchestrationStatusSynchronizer
{
    public async Task SynchronizeForSystemInstancesAsync(
        IEnumerable<Guid> systemInstanceIds,
        CancellationToken cancellationToken = default,
        Guid? excludedOrchestrationId = null)
    {
        ArgumentNullException.ThrowIfNull(systemInstanceIds);
        var affectedOrchestrationIds = new HashSet<Guid>();
        foreach (var systemInstanceId in systemInstanceIds.Distinct())
        {
            cancellationToken.ThrowIfCancellationRequested();
            var associations = await associationRepository.GetBySystemInstanceIdAsync(
                SystemInstanceId.Create(systemInstanceId),
                cancellationToken: cancellationToken);
            foreach (var association in associations)
            {
                if (association.OrchestrationId.Value != excludedOrchestrationId)
                    affectedOrchestrationIds.Add(association.OrchestrationId.Value);
            }
        }

        foreach (var orchestrationId in affectedOrchestrationIds)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var associations = await associationRepository.GetByOrchestrationIdAsync(
                OrchestrationId.Create(orchestrationId),
                cancellationToken: cancellationToken);
            var summary = await resourceCoordinator.GetLifecycleSummaryAsync(
                associations.Select(association => association.SystemInstanceId.Value), cancellationToken);
            if (!Enum.TryParse<OrchestrationOperationalStatus>(summary.OperationalStatus, true, out var targetStatus)
                || !Enum.IsDefined(targetStatus))
                throw new InvalidOperationException($"Resource coordinator returned invalid operational status '{summary.OperationalStatus}'.");
            if (!Enum.TryParse<OrchestrationDesiredState>(summary.DesiredState, true, out var targetDesiredState)
                || !Enum.IsDefined(targetDesiredState))
                throw new InvalidOperationException($"Resource coordinator returned invalid desired state '{summary.DesiredState}'.");
            var lifecycle = await orchestrationLifecycleQueryHandler.GetOrchestrationLifecycleByIdAsync(
                new GetOrchestrationLifecycleByIdQuery { OrchestrationId = orchestrationId },
                cancellationToken) ?? throw new OrchestrationLifecycleNotFoundException(orchestrationId);

            if (lifecycle.OperationalStatus != targetStatus)
            {
                switch (targetStatus)
                {
                    case OrchestrationOperationalStatus.Running:
                        await orchestrationLifecycleUseCaseHandler.MarkOrchestrationRunningAsync(
                            new MarkOrchestrationRunningCommand { OrchestrationId = orchestrationId }, cancellationToken);
                        break;
                    case OrchestrationOperationalStatus.Stopped:
                        await orchestrationLifecycleUseCaseHandler.MarkOrchestrationStoppedAsync(
                            new MarkOrchestrationStoppedCommand { OrchestrationId = orchestrationId }, cancellationToken);
                        break;
                    case OrchestrationOperationalStatus.Failed:
                        await orchestrationLifecycleUseCaseHandler.MarkOrchestrationFailedAsync(
                            new MarkOrchestrationFailedCommand { OrchestrationId = orchestrationId }, cancellationToken);
                        break;
                    case OrchestrationOperationalStatus.Unknown:
                        await orchestrationLifecycleUseCaseHandler.MarkOrchestrationOperationalStatusUnknownAsync(
                            new MarkOrchestrationOperationalStatusUnknownCommand { OrchestrationId = orchestrationId }, cancellationToken);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported orchestration operational status '{targetStatus}'.");
                }
            }

            if (lifecycle.DesiredState != targetDesiredState)
            {
                await orchestrationLifecycleUseCaseHandler.SetOrchestrationDesiredStateAsync(
                    new SetOrchestrationDesiredStateCommand { OrchestrationId = orchestrationId, DesiredState = targetDesiredState }, cancellationToken);
            }
        }
    }

}
