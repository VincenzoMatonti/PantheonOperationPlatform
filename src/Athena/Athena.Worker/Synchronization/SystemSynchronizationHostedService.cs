using Microsoft.Extensions.Options;
using Athena.Worker.Hosting.Services;
using Athena.Worker.Synchronization.Options;
using Athena.Application.BackgroundTasks.Synchronization;

namespace Athena.Worker.Synchronization;

public sealed class SystemSynchronizationHostedService(
    IServiceScopeFactory scopeFactory,
    IOptions<SystemSynchronizationOptions> options,
    ILogger<SystemSynchronizationHostedService> logger)
    : PeriodicBackgroundService<SystemSynchronizationOptions>(options, logger)
{
    protected override async Task ExecuteCycleAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var synchronizationService = scope.ServiceProvider.GetRequiredService<SystemSynchronizationService>();
        await synchronizationService.SynchronizeAsync(cancellationToken);
    }
}