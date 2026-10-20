using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Commands;

public class RenameSystemCommand(SystemRegistryService service) : AsyncCommand<RenameSystemCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<id>")]
        public string Id { get; set; } = string.Empty;

        [CommandOption("--name <NAME>")]
        public string Name { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(settings.Id, out var id))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Invalid system ID.");
            return -1;
        }

        if (string.IsNullOrWhiteSpace(settings.Name))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Specify the new name with --name.");
            return -1;
        }

        try
        {
            var system = await service.RenameAsync(id, new RenameSystemRequest(settings.Name), cancellationToken);
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