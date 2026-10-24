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

        AnsiConsole.Write(new Rule("[bold deepskyblue1]ORCHESTRATION REGISTRY[/]").RuleStyle("deepskyblue1").LeftJustified());
        AnsiConsole.MarkupLine($"[dim]Registered orchestration definitions:[/] [bold white]{items.Count}[/]");
        AnsiConsole.WriteLine();
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.DeepSkyBlue1)
            .Title("[bold deepskyblue1]Orchestrations[/]")
            .Caption("[dim]Use an orchestration ID to inspect or manage its lifecycle.[/]")
            .Expand()
            .AddColumn(new TableColumn("[bold]ID[/]").NoWrap())
            .AddColumn("[bold]Name[/]")
            .AddColumn("[bold]Registration[/]")
            .AddColumn("[bold]Availability[/]");

        foreach (var orchestration in items)
        {
            table.AddRow(
                $"[dim]{orchestration.OrchestrationId}[/]",
                $"[bold white]{Markup.Escape(orchestration.Name)}[/]",
                FormatStatus(orchestration.RegistrationStatus),
                FormatStatus(orchestration.AvailabilityStatus));
        }

        AnsiConsole.Write(table);
    }

    public static void WriteOrchestration(OrchestrationResponse orchestration)
    {
        var title = string.IsNullOrWhiteSpace(orchestration.Name) ? "Orchestration details" : Markup.Escape(orchestration.Name);
        var summary = new Grid().AddColumn().AddColumn();
        summary.AddRow(new Markup("[bold deepskyblue1]REGISTRATION[/]"), new Markup(FormatStatus(orchestration.RegistrationStatus)));
        summary.AddRow(new Markup("[bold deepskyblue1]AVAILABILITY[/]"), new Markup(FormatStatus(orchestration.AvailabilityStatus)));
        summary.AddRow(new Markup("[bold deepskyblue1]DELETED[/]"), new Markup(orchestration.IsDeleted ? "[bold red]Yes[/]" : "[bold green]No[/]"));
        AnsiConsole.Write(new Panel(summary).Header($"[bold deepskyblue1] {title} [/]").Border(BoxBorder.Double).BorderColor(Color.DeepSkyBlue1).Padding(1, 1));
        var metadata = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey).HideHeaders().AddColumn(new TableColumn("[dim]Property[/]").Width(20)).AddColumn("Value");
        metadata.AddRow("[dim]Orchestration ID[/]", $"[white]{orchestration.OrchestrationId}[/]");
        metadata.AddRow("[dim]Created (UTC)[/]", $"[white]{orchestration.CreatedAtUtc:yyyy-MM-dd HH:mm:ss}[/]");
        metadata.AddRow("[dim]Updated (UTC)[/]", orchestration.UpdatedAtUtc is { } updated ? $"[white]{updated:yyyy-MM-dd HH:mm:ss}[/]" : "[dim]-[/]");
        AnsiConsole.Write(metadata);
    }

    public static void WriteMessage(string message)
    {
        AnsiConsole.MarkupLine($"[bold green]✔[/] {Markup.Escape(message)}");
    }

    public static void WriteError(string message)
    {
        AnsiConsole.Write(new Panel(new Markup(Markup.Escape(message))).Header("[bold white on red] ERROR [/]").Border(BoxBorder.Rounded).BorderColor(Color.Red));
    }

    private static string FormatStatus(string? status)
    {
        var value = status ?? "-";
        var escaped = Markup.Escape(value);
        var color = value.Trim().ToLowerInvariant() switch
        {
            "registered" or "available" or "running" or "active" or "enabled" or "completed" or "succeeded" or "success" => "green",
            "unknown" or "pending" or "starting" or "stopping" or "in progress" or "unavailable" => "yellow",
            "unregistered" or "disabled" or "failed" or "error" or "cancelled" or "canceled" => "red",
            _ => "silver"
        };

        return $"[bold {color}]{escaped}[/]";
    }
}
