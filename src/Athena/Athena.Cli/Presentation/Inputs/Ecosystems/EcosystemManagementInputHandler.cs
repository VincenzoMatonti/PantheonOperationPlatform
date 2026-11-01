using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Ecosystems;

public static class EcosystemManagementInputHandler
{
    public static IConfigurator AddEcosystemManagementCommands(this IConfigurator config)
    {
        config.AddBranch("ecosystems", ecosystems =>
        {
            ecosystems.AddBranch("management", management =>
            {
                management.SetDescription("Manage Ecosystem lifecycle state and operations.");
                management.AddCommand<EcosystemManagementCommand>("execute");
            });
        });
        return config;
    }
}
