using Athena.Worker.Reconciliation;
using Athena.Worker.Reconciliation.Options;
using Athena.Worker.Synchronization;
using Athena.Worker.Synchronization.Options;

namespace Athena.Worker.Hosting.Extensions;

public static class WorkerServiceExtensions
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services)
    {
        services.AddOrchestrationReconciliation();
        services.AddSystemSynchronization();
        return services;
    }

    private static IServiceCollection AddOrchestrationReconciliation(this IServiceCollection services)
    {
        services.AddOptions<OrchestrationReconciliationOptions>()
            .BindConfiguration(OrchestrationReconciliationOptions.SectionName)
            .Validate(options => options.Interval > TimeSpan.Zero, "The orchestration reconciliation interval must be greater than zero.")
            .ValidateOnStart();

        services.AddHostedService<OrchestrationReconciliationHostedService>();
        return services;
    }

    private static IServiceCollection AddSystemSynchronization(this IServiceCollection services)
    {
        services.AddOptions<SystemSynchronizationOptions>()
            .BindConfiguration(SystemSynchronizationOptions.SectionName)
            .Validate(options => options.Interval > TimeSpan.Zero, "The system synchronization interval must be greater than zero.")
            .ValidateOnStart();

        services.AddHostedService<SystemSynchronizationHostedService>();
        return services;
    }
}
