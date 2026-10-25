using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.Enums;
using Athena.Application.OrchestrationManagement.Exceptions;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.OrchestrationManagement.UseCases;
using Athena.Application.SystemManagement.Queries;
using Athena.Domain.OrchestrationManagement.Repositories;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Application.Common.Services;

public sealed class OrchestrationOperationalStatusSynchronizationService(
    IOrchestrationSystemRepository orchestrationSystemRepository,
    OrchestrationLifecycleQueryHandler orchestrationLifecycleQueryHandler,
    OrchestrationLifecycleUseCaseHandler orchestrationLifecycleUseCaseHandler,
    SystemManagementQueryHandler systemManagementQueryHandler)
{
    private readonly IOrchestrationSystemRepository _orchestrationSystemRepository = orchestrationSystemRepository;
    private readonly OrchestrationLifecycleQueryHandler _orchestrationLifecycleQueryHandler = orchestrationLifecycleQueryHandler;
    private readonly OrchestrationLifecycleUseCaseHandler _orchestrationLifecycleUseCaseHandler = orchestrationLifecycleUseCaseHandler;
    private readonly SystemManagementQueryHandler _systemManagementQueryHandler = systemManagementQueryHandler;

    public async Task SynchronizeForSystemsAsync(IEnumerable<Guid> systemIds, CancellationToken cancellationToken = default, Guid? excludedOrchestrationId = null)
    {
        ArgumentNullException.ThrowIfNull(systemIds);
        var affectedOrchestrationIds = new HashSet<Guid>();
        foreach (var systemId in systemIds.Distinct())
        {
            cancellationToken.ThrowIfCancellationRequested();
            var associations = await _orchestrationSystemRepository.GetBySystemIdAsync(SystemId.Create(systemId), cancellationToken: cancellationToken);
            foreach (var association in associations)
            {
                var orchestrationId = association.OrchestrationId.Value;
                if (orchestrationId != excludedOrchestrationId) affectedOrchestrationIds.Add(orchestrationId);
            }
        }

        foreach (var orchestrationId in affectedOrchestrationIds)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var associations = await _orchestrationSystemRepository.GetByOrchestrationIdAsync(OrchestrationId.Create(orchestrationId), cancellationToken: cancellationToken);
            var systemStatuses = new List<string>();
            foreach (var association in associations)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var lifecycle = await _systemManagementQueryHandler.GetSystemLifecycleBySystemIdAsync(new GetSystemLifecycleBySystemIdQuery { SystemId = association.SystemId.Value }, cancellationToken);
                if (lifecycle is null)
                {
                    systemStatuses.Add("Unknown");
                    continue;
                }

                systemStatuses.Add(lifecycle.OperationalStatus);
            }

            var targetStatus = DetermineOperationalStatus(systemStatuses);
            var lifecycleQuery = new GetOrchestrationLifecycleByIdQuery { OrchestrationId = orchestrationId };
            var orchestrationLifecycle = await _orchestrationLifecycleQueryHandler.GetOrchestrationLifecycleByIdAsync(lifecycleQuery, cancellationToken) ??
            throw new OrchestrationLifecycleNotFoundException(orchestrationId);
            if (orchestrationLifecycle.OperationalStatus == targetStatus) continue;
            switch (targetStatus)
            {
                case OrchestrationOperationalStatus.Running:
                    await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationRunningAsync(new MarkOrchestrationRunningCommand { OrchestrationId = orchestrationId }, cancellationToken);
                    break;

                case OrchestrationOperationalStatus.Stopped:
                    await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationStoppedAsync(new MarkOrchestrationStoppedCommand { OrchestrationId = orchestrationId }, cancellationToken);
                    break;

                case OrchestrationOperationalStatus.Failed:
                    await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationFailedAsync(new MarkOrchestrationFailedCommand { OrchestrationId = orchestrationId }, cancellationToken);
                    break;

                case OrchestrationOperationalStatus.Unknown:
                    await _orchestrationLifecycleUseCaseHandler.MarkOrchestrationOperationalStatusUnknownAsync(
                        new MarkOrchestrationOperationalStatusUnknownCommand { OrchestrationId = orchestrationId }, cancellationToken);
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported orchestration operational status '{targetStatus}'.");
            }
        }
    }

    private static OrchestrationOperationalStatus DetermineOperationalStatus(List<string> systemStatuses)
    {
        if (systemStatuses.Count == 0) return OrchestrationOperationalStatus.Unknown;
        if (systemStatuses.Any(status => status == "Failed")) return OrchestrationOperationalStatus.Failed;
        if (systemStatuses.All(status => status == "Running")) return OrchestrationOperationalStatus.Running;
        if (systemStatuses.All(status => status == "Stopped")) return OrchestrationOperationalStatus.Stopped;
        return OrchestrationOperationalStatus.Unknown;
    }
}