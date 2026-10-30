using Athena.Application.Common.Services;
using Athena.Application.SystemRegistry.Queries;

namespace Athena.Application.BackgroundTasks.Reconciliation;

public class OrchestrationReconciliationService(SystemRegistryQueryHandler systemRegistryQueryHandler, OrchestrationStatusSynchronizationService orchestrationStatusSynchronizationService)
{
    public async Task ReconcileAsync(CancellationToken cancellationToken = default)
    {
        var systems = await systemRegistryQueryHandler.GetAllSystemsAsync(cancellationToken);
        var systemIds = systems.Where(system => !system.IsDeleted).Select(system => system.Id.Value).Distinct().ToArray();
        if (systemIds.Length == 0) return;
        await orchestrationStatusSynchronizationService.SynchronizeForSystemsAsync(systemIds, cancellationToken);
    }
}