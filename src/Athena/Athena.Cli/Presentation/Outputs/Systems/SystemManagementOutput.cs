using Athena.Cli.Application.Contracts.Systems;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Systems;

public static class SystemManagementOutput
{
    public static void RenderLifecycle(SystemLifecycleResponse lifecycle)
    {
        AnsiConsole.Write(new Rule("[bold cyan]SYSTEM LIFECYCLE[/]").RuleStyle("cyan").LeftJustified());
        var overview = new Table().Border(TableBorder.None).HideHeaders().AddColumn(new TableColumn("Property").Width(20)).AddColumn("Value");
        overview.AddRow("[bold]DESIRED STATE[/]", FormatStatus(lifecycle.DesiredState));
        overview.AddRow("[bold]OPERATIONAL STATUS[/]", FormatStatus(lifecycle.OperationalStatus));
        overview.AddRow("[bold]DELETED[/]", lifecycle.IsDeleted ? "[red]Yes[/]" : "[green]No[/]");
        AnsiConsole.Write(new Panel(overview).Header("[bold cyan]Lifecycle overview[/]").Border(BoxBorder.Double).BorderColor(Color.Cyan1).Padding(1, 1));
        var metadata = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey).AddColumn(new TableColumn("[bold]Property[/]")).AddColumn(new TableColumn("[bold]Value[/]"));
        metadata.AddRow("System ID", $"[dim]{lifecycle.SystemId}[/]");
        metadata.AddRow("Created (UTC)", FormatTimestamp(lifecycle.CreatedAtUtc));
        metadata.AddRow("Updated (UTC)", FormatTimestamp(lifecycle.UpdatedAtUtc));
        AnsiConsole.Write(metadata);
    }

    public static void RenderLifecycles(IReadOnlyList<SystemLifecycleResponse> lifecycles)
    {
        AnsiConsole.Write(new Rule("[bold cyan]SYSTEM LIFECYCLES[/]").RuleStyle("cyan").LeftJustified());
        if (lifecycles.Count == 0)
        {
            WriteEmpty("No system lifecycles found.");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Cyan1)
            .Title("[bold cyan]Lifecycle Overview[/]")
            .Caption($"[dim]{lifecycles.Count} lifecycle record(s)[/]")
            .Expand()
            .AddColumn(new TableColumn("[bold]System ID[/]").NoWrap())
            .AddColumn(new TableColumn("[bold]Desired State[/]"))
            .AddColumn(new TableColumn("[bold]Operational Status[/]"))
            .AddColumn(new TableColumn("[bold]Created (UTC)[/]"))
            .AddColumn(new TableColumn("[bold]Deleted[/]"));

        foreach (var lifecycle in lifecycles)
        {
            table.AddRow(
                $"[dim]{lifecycle.SystemId}[/]",
                FormatStatus(lifecycle.DesiredState),
                FormatStatus(lifecycle.OperationalStatus),
                FormatTimestamp(lifecycle.CreatedAtUtc),
                lifecycle.IsDeleted ? "[red]Yes[/]" : "[green]No[/]");
        }

        AnsiConsole.Write(table);
    }

    public static void RenderOperation(SystemLifecycleOperationResponse operation)
    {
        AnsiConsole.Write(new Rule("[bold cyan]SYSTEM LIFECYCLE OPERATION[/]").RuleStyle("cyan").LeftJustified());
        var overview = new Table().Border(TableBorder.None).HideHeaders().AddColumn(new TableColumn("Property").Width(20)).AddColumn("Value");
        overview.AddRow("[bold]TYPE[/]", Markup.Escape(operation.Type));
        overview.AddRow("[bold]STATUS[/]", FormatStatus(operation.Status));
        overview.AddRow("[bold]DELETED[/]", operation.IsDeleted ? "[red]Yes[/]" : "[green]No[/]");
        AnsiConsole.Write(new Panel(overview).Header("[bold cyan]Operation overview[/]").Border(BoxBorder.Double).BorderColor(Color.Cyan1).Padding(1, 1));
        var metadata = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey).AddColumn(new TableColumn("[bold]Property[/]")).AddColumn(new TableColumn("[bold]Value[/]"));
        metadata.AddRow("Operation ID", $"[dim]{operation.Id}[/]");
        metadata.AddRow("System ID", $"[dim]{operation.SystemId}[/]");
        metadata.AddRow("Created (UTC)", FormatTimestamp(operation.CreatedAtUtc));
        metadata.AddRow("Updated (UTC)", FormatTimestamp(operation.UpdatedAtUtc));
        metadata.AddRow("Started (UTC)", FormatTimestamp(operation.StartedAtUtc));
        metadata.AddRow("Completed (UTC)", FormatTimestamp(operation.CompletedAtUtc));
        AnsiConsole.Write(metadata);
        if (!string.IsNullOrWhiteSpace(operation.ErrorMessage))
        {
            AnsiConsole.Write(
                new Panel($"[red]{Markup.Escape(operation.ErrorMessage)}[/]")
                    .Header("[bold red]Operation error[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Red)
                    .Padding(1, 0));
        }
    }

    public static void RenderOperations(IReadOnlyList<SystemLifecycleOperationResponse> operations)
    {
        AnsiConsole.Write(new Rule("[bold cyan]SYSTEM LIFECYCLE OPERATIONS[/]").RuleStyle("cyan").LeftJustified());
        if (operations.Count == 0)
        {
            WriteEmpty("No lifecycle operations found.");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Cyan1)
            .Title("[bold cyan]Operation Overview[/]")
            .Caption($"[dim]{operations.Count} operation(s)[/]")
            .Expand()
            .AddColumn(new TableColumn("[bold]Operation ID[/]").NoWrap())
            .AddColumn(new TableColumn("[bold]System ID[/]").NoWrap())
            .AddColumn(new TableColumn("[bold]Type[/]"))
            .AddColumn(new TableColumn("[bold]Status[/]"))
            .AddColumn(new TableColumn("[bold]Created (UTC)[/]"))
            .AddColumn(new TableColumn("[bold]Deleted[/]"));

        foreach (var operation in operations)
        {
            table.AddRow($"[dim]{operation.Id}[/]", $"[dim]{operation.SystemId}[/]", Markup.Escape(operation.Type), FormatStatus(operation.Status), FormatTimestamp(operation.CreatedAtUtc), operation.IsDeleted ? "[red]Yes[/]" : "[green]No[/]");
        }

        AnsiConsole.Write(table);
    }

    public static void RenderSuccess(string message)
    {
        AnsiConsole.MarkupLine($"[bold green]✔[/] [green]{Markup.Escape(message)}[/]");
    }

    private static string FormatStatus(string? status)
    {
        var value = string.IsNullOrWhiteSpace(status) ? "Unknown" : status;
        var color = value.ToLowerInvariant() switch
        {
            "registered" or "available" or "enabled" or "running" or "completed" or "succeeded" or "success" => "green",
            "unknown" or "pending" or "starting" or "stopping" or "unavailable" => "yellow",
            "disabled" or "unregistered" or "stopped" or "failed" or "error" or "cancelled" or "canceled" => "red",
            _ => "white"
        };

        return $"[{color}]{Markup.Escape(value)}[/]";
    }

    private static string FormatTimestamp(DateTime? timestamp)
    {
        return timestamp?.ToString("yyyy-MM-dd HH:mm:ss") ?? "-";
    }

    private static string FormatTimestamp(DateTime timestamp)
    {
        return timestamp.ToString("yyyy-MM-dd HH:mm:ss");
    }

    private static void WriteEmpty(string message)
    {
        AnsiConsole.Write(new Panel($"[yellow]{Markup.Escape(message)}[/]").Border(BoxBorder.Rounded).BorderColor(Color.Grey).Padding(1, 0));
    }
}
