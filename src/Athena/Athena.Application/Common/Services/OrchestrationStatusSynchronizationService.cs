using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.Enums;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Interfaces;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.OrchestrationManagement.UseCases;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Application.Common.Services;

public sealed class OrchestrationStatusSynchronizationService(
    IOrchestrationSystemInstanceRepository associationRepository,
    ISystemInstanceRepository systemInstanceRepository,
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
            var operationalStatuses = new List<string>();
            var desiredStates = new List<string>();
            foreach (var association in associations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var instance = await systemInstanceRepository.GetByIdAsync(association.SystemInstanceId, cancellationToken);
                operationalStatuses.Add(instance?.OperationalStatus.ToString() ?? "Unknown");
                desiredStates.Add(instance?.DesiredState.ToString() ?? "Unknown");
            }

            var targetStatus = DetermineOperationalStatus(operationalStatuses);
            var targetDesiredState = DetermineDesiredState(desiredStates);
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

    private static OrchestrationOperationalStatus DetermineOperationalStatus(IReadOnlyCollection<string> statuses)
    {
        if (statuses.Count == 0) return OrchestrationOperationalStatus.Unknown;
        if (statuses.Any(status => status is "Failed" or "Unreachable")) return OrchestrationOperationalStatus.Failed;
        if (statuses.All(status => status == "Running")) return OrchestrationOperationalStatus.Running;
        if (statuses.All(status => status == "Stopped")) return OrchestrationOperationalStatus.Stopped;
        return OrchestrationOperationalStatus.Unknown;
    }

    private static OrchestrationDesiredState DetermineDesiredState(IReadOnlyCollection<string> states)
    {
        if (states.Count == 0) return OrchestrationDesiredState.Unknown;
        if (states.All(state => state == "Running")) return OrchestrationDesiredState.Running;
        if (states.All(state => state == "Stopped")) return OrchestrationDesiredState.Stopped;
        return OrchestrationDesiredState.Unknown;
    }
}
