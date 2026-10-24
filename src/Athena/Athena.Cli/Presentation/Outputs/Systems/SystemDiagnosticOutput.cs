using Athena.Cli.Application.Contracts.Systems;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Systems;

public static class SystemDiagnosticOutput
{
    public static void RenderRuntimeStatus(RuntimeResourceStatusResponse status)
    {
        AnsiConsole.Write(new Rule("[bold blue]RUNTIME DIAGNOSTICS[/]").RuleStyle("blue"));
        AnsiConsole.WriteLine();
        var state = GetStatusStyle(status.State);
        var grid = new Grid().AddColumn().AddColumn();
        grid.AddRow(new Markup("[bold]Current state[/]"), new Markup($"[{state}]{Markup.Escape(DisplayValue(status.State))}[/]"));
        grid.AddRow(new Markup("[bold]Running[/]"), new Markup(status.IsRunning ? "[green]Yes[/]" : "[red]No[/]"));
        grid.AddRow(new Markup("[bold]Started at (UTC)[/]"), new Markup(Markup.Escape(status.StartedAtUtc?.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff") ?? "Not available")));
        grid.AddRow(new Markup("[bold]Observed at (UTC)[/]"), new Markup(Markup.Escape(status.ObservedAtUtc.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"))));
        if (!string.IsNullOrWhiteSpace(status.Details))
            grid.AddRow(new Markup("[bold]Details[/]"), new Markup(Markup.Escape(status.Details)));
        var panel = new Panel(grid).Header("[bold]Runtime Status[/]").Border(BoxBorder.Rounded).BorderColor(GetStatusColor(status.State));
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    public static void RenderHealthCheck(RuntimeHealthCheckResponse health)
    {
        AnsiConsole.Write(new Rule("[bold blue]RUNTIME HEALTH CHECK[/]").RuleStyle("blue"));
        AnsiConsole.WriteLine();
        var resultColor = health.IsHealthy ? "green" : "red";
        var resultIcon = health.IsHealthy ? "[green]✔ HEALTHY[/]" : "[red]✖ UNHEALTHY[/]";
        var borderColor = health.IsHealthy ? Color.Green : Color.Red;
        var grid = new Grid().AddColumn().AddColumn();
        grid.AddRow(new Markup("[bold]Health result[/]"), new Markup($"[{resultColor}]{resultIcon}[/]"));
        grid.AddRow(new Markup("[bold]Status[/]"), new Markup(Markup.Escape(DisplayValue(health.Status))));
        grid.AddRow(new Markup("[bold]Checked at (UTC)[/]"), new Markup(Markup.Escape(health.CheckedAtUtc.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"))));        
        if (!string.IsNullOrWhiteSpace(health.Details)) grid.AddRow(new Markup("[bold]Details[/]"), new Markup(Markup.Escape(health.Details)));
        var panel = new Panel(grid).Header("[bold]Health Report[/]").Border(BoxBorder.Rounded).BorderColor(borderColor);
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
        if (!health.IsHealthy)
            AnsiConsole.MarkupLine("[red bold]Warning:[/] The runtime health check reported a problem.");
        else
            AnsiConsole.MarkupLine("[green]The runtime passed the health check.[/]");

        AnsiConsole.WriteLine();
    }

    public static void RenderLogs(IReadOnlyList<RuntimeLogEntryResponse> logs)
    {
        AnsiConsole.Write(new Rule("[bold blue]RUNTIME LOGS[/]").RuleStyle("blue"));
        AnsiConsole.WriteLine();
        if (logs.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No runtime logs found for the requested filters.[/]");
            AnsiConsole.WriteLine();
            return;
        }

        var table = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey).Title("[bold]Runtime Log Entries[/]")
            .AddColumn(new TableColumn("[grey]Timestamp (UTC)[/]").NoWrap())
            .AddColumn(new TableColumn("[bold]Stream[/]").Centered())
            .AddColumn(new TableColumn("[bold]Message[/]"));

        foreach (var log in logs)
        {
            table.AddRow(
                Markup.Escape(log.TimestampUtc.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff")),
                Markup.Escape(DisplayValue(log.Stream)),
                Markup.Escape(string.IsNullOrWhiteSpace(log.Message) ? "(empty message)" : log.Message));
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        var summary = new Panel(new Markup(
            $"[bold]Entries returned:[/] [cyan]{logs.Count}[/]")).Border(BoxBorder.Rounded).BorderColor(Color.Grey).Padding(1, 0);
        AnsiConsole.Write(summary);
        AnsiConsole.WriteLine();
    }

    public static void RenderSuccess(string message)
    {
        AnsiConsole.MarkupLine($"[green]✔ {Markup.Escape(message)}[/]");
    }

    public static void RenderError(string message)
    {
        AnsiConsole.MarkupLine($"[red]✖ Error: {Markup.Escape(message)}[/]");
    }

    private static string GetStatusStyle(string? status)
    {
        return NormalizeStatus(status) switch
        {
            "available" or "running" or "healthy" => "green",
            "unavailable" or "failed" or "unhealthy" or "stopped" => "red",
            "starting" or "stopping" or "degraded" => "yellow",
            "unknown" => "grey",
            _ => "white"
        };
    }

    private static Color GetStatusColor(string? status)
    {
        return NormalizeStatus(status) switch
        {
            "available" or "running" or "healthy" => Color.Green,
            "unavailable" or "failed" or "unhealthy" or "stopped" => Color.Red,
            "starting" or "stopping" or "degraded" => Color.Yellow,
            "unknown" => Color.Grey,
            _ => Color.Blue
        };
    }

    private static string NormalizeStatus(string? status) => status?.Trim().ToLowerInvariant() ?? string.Empty;
    private static string DisplayValue(string? value) => string.IsNullOrWhiteSpace(value) ? "Unknown" : value;
}
