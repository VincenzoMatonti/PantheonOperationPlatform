using Athena.Worker.Reconciliation;
using Athena.Worker.Reconciliation.Options;
using Athena.Worker.Synchronization;
using Athena.Worker.Synchronization.Options;
using Athena.Worker.Operations;
using Athena.Worker.Operations.Options;

namespace Athena.Worker.Hosting.Extensions;

public static class WorkerServiceExtensions
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services)
    {
        services.AddOrchestrationReconciliation();
        services.AddSystemSynchronization();
        services.AddSystemInstanceOperationProcessing();
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

    private static IServiceCollection AddSystemInstanceOperationProcessing(this IServiceCollection services)
    {
        services.AddOptions<SystemInstanceOperationsOptions>()
            .BindConfiguration(SystemInstanceOperationsOptions.SectionName)
            .Validate(options => options.PollingInterval > TimeSpan.Zero, "The SystemInstance operation polling interval must be greater than zero.")
            .Validate(options => options.LeaseDuration > TimeSpan.Zero, "The SystemInstance operation lease duration must be greater than zero.")
            .Validate(options => string.Equals(options.AgentProtocol, "Legacy", StringComparison.OrdinalIgnoreCase)
                || string.Equals(options.AgentProtocol, "Correlated", StringComparison.OrdinalIgnoreCase),
                "The SystemInstance operation Agent protocol must be Correlated or Legacy.")
            .ValidateOnStart();
        services.AddHostedService<SystemInstanceOperationsHostedService>();
        return services;
    }
}
