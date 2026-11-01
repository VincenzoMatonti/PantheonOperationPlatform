using Athena.Cli.Presentation.Inputs.Systems.Instances.Commands;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Instances;

public static class SystemInstanceInputHandler
{
    public static IConfigurator<CommandSettings> AddSystemInstanceCommands(this IConfigurator<CommandSettings> config)
    {
        config.AddBranch("instances", instances =>
        {
            instances.SetDescription("Manage individual SystemInstances.");
            instances.AddCommand<ListSystemInstancesCommand>("list");
            instances.AddCommand<GetSystemInstanceCommand>("get");
            instances.AddCommand<CreateSystemInstanceCommand>("create");
            instances.AddCommand<RegisterSystemInstanceCommand>("register");
            instances.AddCommand<StartSystemInstanceCommand>("start");
            instances.AddCommand<StopSystemInstanceCommand>("stop");
            instances.AddCommand<RestartSystemInstanceCommand>("restart");
            instances.AddCommand<RemoveSystemInstanceCommand>("remove");
            instances.AddCommand<GetSystemInstanceStatusCommand>("status");
            instances.AddCommand<GetSystemInstanceHealthCommand>("health");
            instances.AddCommand<GetSystemInstanceLogsCommand>("logs");
            instances.AddCommand<GetSystemInstanceOperationsCommand>("operations");
            instances.AddCommand<GetSystemInstanceOperationCommand>("operation");
        });

        return config;
    }
}
