using Athena.Cli.Application.Contracts.Orchestrations;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Orchestrations;

public static class OrchestrationManagementOutput
{
    public static void WriteLifecycle(OrchestrationLifecycleResponse lifecycle)
    {
        AnsiConsole.Write(new Rule("[bold cyan]ORCHESTRATION LIFECYCLE[/]").RuleStyle("cyan").LeftJustified());
        var summary = new Grid().AddColumn().AddColumn();
        summary.AddRow(new Markup("[bold cyan]DESIRED STATE[/]"), new Markup(FormatStatus(lifecycle.DesiredState)));
        summary.AddRow(new Markup("[bold cyan]OPERATIONAL STATUS[/]"), new Markup(FormatStatus(lifecycle.OperationalStatus)));
        summary.AddRow(new Markup("[bold cyan]DELETED[/]"), new Markup(lifecycle.IsDeleted ? "[bold red]Yes[/]" : "[bold green]No[/]"));
        AnsiConsole.Write(new Panel(summary).Header("[bold cyan] Lifecycle overview [/]").Border(BoxBorder.Double).BorderColor(Color.Cyan).Padding(1, 1));
        var metadata = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey).HideHeaders().AddColumn("Property").AddColumn("Value");
        metadata.AddRow("[dim]Orchestration ID[/]", $"[white]{lifecycle.OrchestrationId}[/]");
        metadata.AddRow("[dim]Created (UTC)[/]", $"[white]{FormatDate(lifecycle.CreatedAtUtc)}[/]");
        metadata.AddRow("[dim]Updated (UTC)[/]", $"[white]{FormatDate(lifecycle.UpdatedAtUtc)}[/]");

        AnsiConsole.Write(metadata);
    }

    public static void WriteLifecycles(IEnumerable<OrchestrationLifecycleResponse> lifecycles)
    {
        var items = lifecycles.ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No lifecycles found.[/]");
            return;
        }

        AnsiConsole.Write(new Rule("[bold cyan]LIFECYCLE OVERVIEW[/]").RuleStyle("cyan").LeftJustified());
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Cyan)
            .Title("[bold cyan]Orchestration Lifecycles[/]")
            .Caption($"[dim]{items.Count} lifecycle record(s)[/]")
            .Expand()
            .AddColumn("[bold]Orchestration ID[/]")
            .AddColumn("[bold]Desired State[/]")
            .AddColumn("[bold]Operational Status[/]")
            .AddColumn("[bold]Created (UTC)[/]")
            .AddColumn("[bold]Deleted[/]");

        foreach (var lifecycle in items)
        {
            table.AddRow(
                $"[dim]{lifecycle.OrchestrationId}[/]",
                FormatStatus(lifecycle.DesiredState),
                FormatStatus(lifecycle.OperationalStatus),
                FormatDate(lifecycle.CreatedAtUtc),
                lifecycle.IsDeleted ? "[red]Yes[/]" : "[green]No[/]");
        }

        AnsiConsole.Write(table);
    }

    public static void WriteOperation(OrchestrationLifecycleOperationResponse operation)
    {
        var failed = IsFailure(operation.Status) || !string.IsNullOrWhiteSpace(operation.ErrorMessage);
        var completed = IsSuccess(operation.Status) && !failed;
        var borderColor = failed ? Color.Red : completed ? Color.Green : Color.Yellow;
        var titleColor = failed ? "red" : completed ? "green" : "yellow";
        var summary = new Grid().AddColumn().AddColumn();
        summary.AddRow(new Markup("[bold cyan]TYPE[/]"), new Markup(Markup.Escape(operation.Type)));
        summary.AddRow(new Markup("[bold cyan]STATUS[/]"), new Markup(FormatStatus(operation.Status)));
        summary.AddRow(new Markup("[bold cyan]OPERATION ID[/]"), new Markup($"[white]{operation.Id}[/]"));
        summary.AddRow(new Markup("[bold cyan]ORCHESTRATION ID[/]"), new Markup($"[white]{operation.OrchestrationId}[/]"));
        AnsiConsole.Write(new Panel(summary).Header($"[bold {titleColor}] Lifecycle operation [/]").Border(BoxBorder.Double).BorderColor(borderColor).Padding(1, 1));
        var timeline = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey).Title("[bold]Execution timeline[/]").AddColumn("[bold]Event[/]").AddColumn("[bold]Timestamp (UTC)[/]");
        timeline.AddRow("Created", FormatDate(operation.CreatedAtUtc));
        timeline.AddRow("Updated", FormatDate(operation.UpdatedAtUtc));
        timeline.AddRow("Started", FormatDate(operation.StartedAtUtc));
        timeline.AddRow("Completed", FormatDate(operation.CompletedAtUtc));
        AnsiConsole.Write(timeline);
        if (!string.IsNullOrWhiteSpace(operation.ErrorMessage))
            AnsiConsole.Write(new Panel(new Markup(Markup.Escape(operation.ErrorMessage))).Header("[bold white on red] ERROR DETAILS [/]").Border(BoxBorder.Rounded).BorderColor(Color.Red));

        if (operation.IsDeleted)
            AnsiConsole.MarkupLine("[dim]This operation is marked as deleted.[/]");
    }

    public static void WriteOperations(IEnumerable<OrchestrationLifecycleOperationResponse> operations)
    {
        var items = operations.ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No lifecycle operations found.[/]");
            return;
        }

        AnsiConsole.Write(new Rule("[bold cyan]OPERATION HISTORY[/]").RuleStyle("cyan").LeftJustified());
        var table = new Table().Border(TableBorder.Rounded).BorderColor(Color.Cyan).Title("[bold cyan]Orchestration Lifecycle Operations[/]")
            .Caption($"[dim]{items.Count} operation(s)[/]").Expand().AddColumn("[bold]Operation ID[/]").AddColumn("[bold]Orchestration ID[/]")
            .AddColumn("[bold]Type[/]").AddColumn("[bold]Status[/]").AddColumn("[bold]Created (UTC)[/]").AddColumn("[bold]Deleted[/]");

        foreach (var operation in items)
        {
            var hasError = !string.IsNullOrWhiteSpace(operation.ErrorMessage);
            var status = hasError && !IsFailure(operation.Status) ? $"[bold red]{Markup.Escape(operation.Status)}[/]" : FormatStatus(operation.Status);
            table.AddRow($"[dim]{operation.Id}[/]", $"[dim]{operation.OrchestrationId}[/]",
                Markup.Escape(operation.Type), status,
                FormatDate(operation.CreatedAtUtc), operation.IsDeleted ? "[red]Yes[/]" : "[green]No[/]");
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
    private static string FormatStatus(string? status)
    {
        var value = status ?? "-";
        var escaped = Markup.Escape(value);
        var color = value.Trim().ToLowerInvariant() switch
        {
            "available" or "running" or "started" or "active" or "enabled" or "completed" or "succeeded" or "success" => "green",
            "unknown" or "pending" or "starting" or "stopping" or "in progress" or "unavailable" => "yellow",
            "stopped" or "disabled" or "failed" or "error" or "cancelled" or "canceled" => "red",
            _ => "silver"
        };

        return $"[bold {color}]{escaped}[/]";
    }

    private static bool IsFailure(string? status) => status?.Trim().ToLowerInvariant() is "failed" or "failure" or "error" or "cancelled" or "canceled";
    private static bool IsSuccess(string? status) => status?.Trim().ToLowerInvariant() is "completed" or "succeeded" or "success";
}
