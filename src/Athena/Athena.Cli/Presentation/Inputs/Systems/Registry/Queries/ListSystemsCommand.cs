using Athena.Cli.Application.Services.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;

public class ListSystemsCommand(SystemRegistryService systemRegistryService) : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var systems = await systemRegistryService.GetAllAsync(cancellationToken);
        if (systems.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No systems found.[/]");
            return 0;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Registered Systems[/]")
            .AddColumn("ID")
            .AddColumn("Name")
            .AddColumn("Registration")
            .AddColumn("Availability");

        foreach (var system in systems)
        {
            table.AddRow(
                system.Id.ToString(),
                Markup.Escape(system.Name),
                Markup.Escape(system.RegistrationStatus),
                Markup.Escape(system.AvailabilityStatus));
        }

        AnsiConsole.Write(table);
        return 0;
    }
}
