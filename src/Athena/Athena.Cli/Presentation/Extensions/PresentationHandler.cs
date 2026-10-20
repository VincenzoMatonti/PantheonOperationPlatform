using Athena.Cli.Presentation.Inputs.Systems.Registry;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Extensions;

public static class PresentationHandler
{
    public static IConfigurator AddAthenaCliCommands(this IConfigurator config)
    {
        config.AddSystemCommands();
        // config.AddOrchestrationRegistryCommands();
        // config.AddOrchestrationManagementCommands();

        return config;
    }
}