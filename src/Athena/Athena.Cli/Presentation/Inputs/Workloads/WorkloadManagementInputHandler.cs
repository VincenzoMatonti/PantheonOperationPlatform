using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Workloads;

public static class WorkloadManagementInputHandler
{
    public static IConfigurator AddWorkloadManagementCommands(this IConfigurator config)
    {
        config.AddBranch("workloads", workloads =>
        {
            workloads.AddBranch("management", management =>
            {
                management.SetDescription("Manage Workload lifecycle state and operations.");
                management.AddCommand<WorkloadManagementCommand>("execute");
            });
        });
        return config;
    }
}
