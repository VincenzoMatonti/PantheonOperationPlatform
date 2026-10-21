using Athena.Cli.Application.Contracts.Orchestrations;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Orchestrations;

public static class OrchestrationRegistryOutput
{
    public static void WriteOrchestrations(IEnumerable<OrchestrationResponse> orchestrations)
    {
        var items = orchestrations.ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No orchestrations found.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Orchestrations[/]")
            .AddColumn("ID")
            .AddColumn("Name")
            .AddColumn("Registration")
            .AddColumn("Availability");

        foreach (var orchestration in items)
        {
            table.AddRow(
                orchestration.Id.ToString(),
                Markup.Escape(orchestration.Name),
                Markup.Escape(orchestration.RegistrationStatus),
                Markup.Escape(orchestration.AvailabilityStatus));
        }

        AnsiConsole.Write(table);
    }

    public static void WriteOrchestration(OrchestrationResponse orchestration)
    {
        var details = new Table().Border(TableBorder.Simple).HideHeaders().AddColumn("Property").AddColumn("Value");
        details.AddRow("ID", orchestration.Id.ToString());
        details.AddRow("Name", Markup.Escape(orchestration.Name));
        details.AddRow("Registration", Markup.Escape(orchestration.RegistrationStatus));
        details.AddRow("Availability", Markup.Escape(orchestration.AvailabilityStatus));
        details.AddRow("Created (UTC)", orchestration.CreatedAtUtc.ToString("O"));
        details.AddRow("Updated (UTC)", orchestration.UpdatedAtUtc?.ToString("O") ?? "-");
        details.AddRow("Deleted", orchestration.IsDeleted.ToString());
        AnsiConsole.Write(new Panel(details).Header("[bold]Orchestration details[/]").Border(BoxBorder.Rounded));
    }

    public static void WriteMessage(string message)
    {
        AnsiConsole.MarkupLine($"[green]{Markup.Escape(message)}[/]");
    }

    public static void WriteError(string message)
    {
        AnsiConsole.MarkupLine($"[red]Error:[/] {Markup.Escape(message)}");
    }
}
