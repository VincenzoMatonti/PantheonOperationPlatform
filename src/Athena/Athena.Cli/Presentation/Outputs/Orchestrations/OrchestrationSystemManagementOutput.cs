using Athena.Cli.Application.Contracts.Orchestrations;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Orchestrations;

public static class OrchestrationSystemManagementOutput
{
    public static void Write(OrchestrationSystemResponse association)
    {
        AnsiConsole.Write(new Rule("[bold mediumorchid]ORCHESTRATION ↔ SYSTEM[/]").RuleStyle("mediumorchid").LeftJustified());
        var summary = new Grid().AddColumn().AddColumn();
        summary.AddRow(new Markup("[bold mediumorchid]ASSOCIATION[/]"), new Markup("[bold green]Registered[/]"));
        summary.AddRow(new Markup("[bold mediumorchid]DELETED[/]"), new Markup(association.IsDeleted ? "[bold red]Yes[/]" : "[bold green]No[/]"));
        AnsiConsole.Write(new Panel(summary).Header("[bold mediumorchid] System association [/]").Border(BoxBorder.Double).BorderColor(Color.MediumPurple1).Padding(1, 1));
        var identifiers = new Table().Border(TableBorder.Rounded).BorderColor(Color.MediumPurple1).Title("[bold mediumorchid]Identifiers[/]")
                                     .AddColumn("[bold]Entity[/]").AddColumn("[bold]ID[/]");
        identifiers.AddRow("[bold]Association[/]", $"[white]{association.Id}[/]");
        identifiers.AddRow("[bold]Orchestration[/]", $"[deepskyblue1]{association.OrchestrationId}[/]");
        identifiers.AddRow("[bold]System[/]", $"[green]{association.SystemId}[/]");
        AnsiConsole.Write(identifiers);
        var metadata = new Table().Border(TableBorder.Simple).BorderColor(Color.Grey).HideHeaders().AddColumn("Property").AddColumn("Value");
        metadata.AddRow("[dim]Created (UTC)[/]", $"[white]{FormatDate(association.CreatedAtUtc)}[/]");
        metadata.AddRow("[dim]Updated (UTC)[/]", FormatDate(association.UpdatedAtUtc));

        AnsiConsole.Write(metadata);
    }

    public static void WriteMany(IEnumerable<OrchestrationSystemResponse> associations)
    {
        var items = associations.ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No orchestration system associations found.[/]");
            return;
        }

        AnsiConsole.Write(new Rule("[bold mediumorchid]ORCHESTRATION SYSTEM LINKS[/]").RuleStyle("mediumorchid").LeftJustified());
        AnsiConsole.MarkupLine($"[dim]Total associations:[/] [bold white]{items.Count}[/]");
        var table = new Table()
            .Border(TableBorder.Rounded).BorderColor(Color.MediumPurple1).Title("[bold mediumorchid]System Associations[/]")
            .Caption("[dim]Association ID identifies the link; the other IDs identify the related entities.[/]").Expand().AddColumn("[bold]Association ID[/]")
            .AddColumn("[bold]Orchestration ID[/]").AddColumn("[bold]System ID[/]").AddColumn("[bold]Created (UTC)[/]").AddColumn("[bold]Deleted[/]");

        foreach (var association in items)
        {
            table.AddRow($"[dim]{association.Id}[/]", $"[deepskyblue1]{association.OrchestrationId}[/]", $"[green]{association.SystemId}[/]",
                FormatDate(association.CreatedAtUtc), association.IsDeleted ? "[red]Yes[/]" : "[green]No[/]");
        }

        AnsiConsole.Write(table);
    }

    public static void WriteMessage(string message)
    {
        AnsiConsole.MarkupLine($"[bold green]✔[/] {Markup.Escape(message)}");
    }

    public static void WriteError(string message)
    {
        AnsiConsole.Write(new Panel(new Markup(Markup.Escape(message))).Header("[bold white on red] ERROR [/]").Border(BoxBorder.Rounded).BorderColor(Color.Red));
    }

    private static string FormatDate(DateTime value) => value.ToString("yyyy-MM-dd HH:mm:ss");
    private static string FormatDate(DateTime? value) => value.HasValue ? FormatDate(value.Value) : "[dim]-[/]";
}
