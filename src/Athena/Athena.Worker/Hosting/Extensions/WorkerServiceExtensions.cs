using Athena.Worker.Reconciliation;
using Athena.Worker.Reconciliation.Options;

namespace Athena.Worker.Hosting.Extensions;

public static class WorkerServiceExtensions
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services)
    {
        services.AddOrchestrationReconciliation();
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
}
