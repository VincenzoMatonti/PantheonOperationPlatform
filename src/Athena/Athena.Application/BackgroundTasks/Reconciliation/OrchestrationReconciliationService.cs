using Athena.Application.Common.Services;
using Athena.Domain.System.Registry.Repositories;

namespace Athena.Application.BackgroundTasks.Reconciliation;

public class OrchestrationReconciliationService(
    ISystemInstanceRepository systemInstanceRepository,
    OrchestrationStatusSynchronizationService orchestrationStatusSynchronizationService)
{
    public async Task ReconcileAsync(CancellationToken cancellationToken = default)
    {
        var instances = await systemInstanceRepository.GetAllAsync(cancellationToken);
        var systemInstanceIds = instances.Select(instance => instance.Id.Value).Distinct().ToArray();
        if (systemInstanceIds.Length == 0) return;
        await orchestrationStatusSynchronizationService.SynchronizeForSystemInstancesAsync(systemInstanceIds, cancellationToken);
    }
}