using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Blocks;

public static class BlockManagementInputHandler
{
    public static IConfigurator AddBlockManagementCommands(this IConfigurator config)
    {
        config.AddBranch("blocks", blocks =>
        {
            blocks.AddBranch("management", management =>
            {
                management.SetDescription("Manage Block lifecycle state and operations.");
                management.AddCommand<BlockManagementCommand>("execute");
            });
        });
        return config;
    }
}
