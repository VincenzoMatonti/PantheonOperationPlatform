using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Commands;

public class CreateSystemCommand(SystemRegistryService systemRegistryService) : AsyncCommand<CreateSystemCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--name <NAME>")]
        public string Name { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(settings.Name))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Specify a system name with --name.");
            return -1;
        }

        try
        {
            var system = await systemRegistryService.CreateAsync(new RegisterSystemRequest(settings.Name), cancellationToken);
            SystemRegistryOutput.WriteSystem(system);
            return 0;
        }
        catch (Exception exception)
        {
            SystemRegistryOutput.WriteError(exception.Message);
            return -1;
        }
    }
}