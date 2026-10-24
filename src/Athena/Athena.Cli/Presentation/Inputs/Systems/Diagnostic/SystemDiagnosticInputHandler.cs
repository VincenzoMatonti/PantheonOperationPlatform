using Athena.Cli.Presentation.Inputs.Systems.Diagnostic.Commands;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Diagnostic;

public static class SystemDiagnosticInputHandler
{
    public static IConfigurator<CommandSettings> AddSystemDiagnosticCommands(this IConfigurator<CommandSettings> config)
    {
        config.AddBranch("diagnostics", diagnostics =>
        {
            diagnostics.SetDescription("Inspect runtime status, health, and logs.");
            diagnostics.AddCommand<GetRuntimeStatusCommand>("runtime-status").WithDescription("Get the status of a runtime instance.");
            diagnostics.AddCommand<CheckRuntimeHealthCommand>("runtime-health").WithDescription("Check the health of a runtime instance.");
            diagnostics.AddCommand<ReadRuntimeLogsCommand>("runtime-logs").WithDescription("Read logs from a runtime instance.");
        });

        return config;
    }
}