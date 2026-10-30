using Microsoft.Extensions.Options;
using Athena.Worker.Hosting.Services;
using Athena.Worker.Reconciliation.Options;
using Athena.Application.BackgroundTasks.Reconciliation;

namespace Athena.Worker.Reconciliation;

public sealed class OrchestrationReconciliationHostedService(
    IServiceScopeFactory scopeFactory,
    IOptions<OrchestrationReconciliationOptions> options,
    ILogger<OrchestrationReconciliationHostedService> logger)
    : PeriodicBackgroundService<OrchestrationReconciliationOptions>(options, logger)
{
    protected override async Task ExecuteCycleAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var reconciliationService = scope.ServiceProvider.GetRequiredService<OrchestrationReconciliationService>();
        await reconciliationService.ReconcileAsync(cancellationToken);
    }
}
