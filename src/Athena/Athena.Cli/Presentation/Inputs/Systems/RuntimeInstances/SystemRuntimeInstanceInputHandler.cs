using Athena.Cli.Presentation.Inputs.Systems.RuntimeInstances.Commands;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.RuntimeInstances;

public static class SystemRuntimeInstancesInputHandler
{
    public static IConfigurator<CommandSettings> AddSystemRuntimeInstancesCommands(this IConfigurator<CommandSettings> config)
    {
        config.AddBranch("runtime-instances", runtimeInstances =>
        {
            runtimeInstances.SetDescription("Register and inspect system runtime instances.");
            runtimeInstances.AddCommand<ListRuntimeInstancesCommand>("list").WithDescription("List all registered runtime instances.");
            runtimeInstances.AddCommand<GetRuntimeInstanceCommand>("get").WithDescription("Get a runtime instance by ID.");
            runtimeInstances.AddCommand<GetRuntimeInstancesBySystemCommand>("by-system").WithDescription("List runtime instances for a system.");
            runtimeInstances.AddCommand<GetRuntimeInstancesByEnvironmentCommand>("by-environment").WithDescription("List runtime instances for a system and environment.");
            runtimeInstances.AddCommand<GetRuntimeInstancesByResourceNameCommand>("by-resource-name").WithDescription("List runtime instances by runtime resource name.");
            runtimeInstances.AddCommand<RegisterRuntimeInstanceCommand>("register").WithDescription("Register a runtime instance.");
            runtimeInstances.AddCommand<UnregisterRuntimeInstanceCommand>("unregister").WithDescription("Unregister a runtime instance.");
        });

        return config;
    }
}
