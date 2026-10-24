using Athena.Cli.Application.Contracts.Systems;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Systems;

public static class SystemRegistryOutput
{

    public static void WriteSystems(IEnumerable<SystemResponse> systems)
    {
        var items = systems.ToList();
        AnsiConsole.Write(new Rule("[bold cyan]SYSTEM REGISTRY[/]").RuleStyle("cyan").LeftJustified());
        if (items.Count == 0)
        {
            WriteEmpty("No systems registered.");
            return;
        }

        var table = new Table().Border(TableBorder.Rounded).BorderColor(Color.Cyan1).Title("[bold cyan]Registered Systems[/]").Caption($"[dim]{items.Count} system(s)[/]").Expand();
        table.AddColumn(new TableColumn("[bold cyan]System ID[/]").NoWrap());
        table.AddColumn(new TableColumn("[bold cyan]Name[/]"));
        table.AddColumn(new TableColumn("[bold cyan]Registration[/]"));
        table.AddColumn(new TableColumn("[bold cyan]Availability[/]"));

        foreach (var system in items)
        {
            table.AddRow(
                new Markup($"[dim]{Markup.Escape(system.Id.ToString())}[/]"),
                new Markup($"[bold white]{Markup.Escape(system.Name)}[/]"),
                new Markup(FormatStatus(system.RegistrationStatus)),
                new Markup(FormatStatus(system.AvailabilityStatus)));
        }

        AnsiConsole.Write(table);
    }


    public static void WriteSystem(SystemResponse system)
    {
        AnsiConsole.Write(new Rule("[bold cyan]SYSTEM DETAILS[/]").RuleStyle("cyan").LeftJustified());
        var overview = new Table().Border(TableBorder.None).HideHeaders().AddColumn(new TableColumn("Property").Width(18)).AddColumn("Value");
        overview.AddRow("[bold]REGISTRATION[/]", FormatStatus(system.RegistrationStatus));
        overview.AddRow("[bold]AVAILABILITY[/]", FormatStatus(system.AvailabilityStatus));
        overview.AddRow("[bold]DELETED[/]", system.IsDeleted ? "[red]Yes[/]" : "[green]No[/]");
        AnsiConsole.Write(new Panel(overview).Header($"[bold cyan]{Markup.Escape(system.Name)}[/]").Border(BoxBorder.Double).BorderColor(Color.Cyan1).Padding(1, 1));
        var metadata = new Table().Border(TableBorder.Rounded).BorderColor(Color.Grey).AddColumn(new TableColumn("[bold]Property[/]")).AddColumn(new TableColumn("[bold]Value[/]"));
        metadata.AddRow("System ID", $"[dim]{system.Id}[/]");
        metadata.AddRow("Created (UTC)", FormatTimestamp(system.CreatedAtUtc));
        metadata.AddRow("Updated (UTC)", FormatTimestamp(system.UpdatedAtUtc));
        AnsiConsole.Write(metadata);
        WriteEndpoints(system.Endpoints);
        WriteCapabilities(system.Capabilities);
    }

    public static void WriteMessage(string message)
    {
        AnsiConsole.MarkupLine($"[bold green]✔[/] [green]{Markup.Escape(message)}[/]");
    }

    public static void WriteError(string message)
    {
        AnsiConsole.Write(new Panel($"[red]{Markup.Escape(message)}[/]").Header("[bold red]✖ ERROR[/]").Border(BoxBorder.Rounded).BorderColor(Color.Red).Padding(1, 0));
    }

    public static void WriteEndpoints(IEnumerable<SystemEndpointResponse> endpoints)
    {
        var items = endpoints.ToList();
        AnsiConsole.Write(new Rule("[bold deepskyblue1]ENDPOINTS[/]").RuleStyle("deepskyblue1").LeftJustified());
        if (items.Count == 0)
        {
            WriteEmpty("No endpoints configured.");
            return;
        }
        var table = new Table().Border(TableBorder.Rounded).BorderColor(Color.DeepSkyBlue1).Expand().AddColumn(new TableColumn("[bold]Name[/]")).AddColumn(new TableColumn("[bold]Base Address[/]"));
        foreach (var endpoint in items)
        {
            table.AddRow(
                $"[bold white]{Markup.Escape(endpoint.Name)}[/]",
                $"[deepskyblue1]{Markup.Escape(endpoint.BaseAddress)}[/]");
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"[dim]{items.Count} endpoint(s)[/]");
    }

    public static void WriteCapabilities(IEnumerable<SystemCapabilityResponse> capabilities)
    {
        var items = capabilities.ToList();
        AnsiConsole.Write(new Rule("[bold mediumorchid]CAPABILITIES[/]").RuleStyle("mediumorchid").LeftJustified());
        if (items.Count == 0)
        {
            WriteEmpty("No capabilities configured.");
            return;
        }
        var table = new Table().Border(TableBorder.Rounded).BorderColor(Color.MediumPurple1).Expand().AddColumn(new TableColumn("[bold]Capability[/]")).AddColumn(new TableColumn("[bold]Description[/]"));
        foreach (var capability in items)
        {
            table.AddRow($"[bold white]{Markup.Escape(capability.Name)}[/]", Markup.Escape(capability.Description ?? "-"));
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"[dim]{items.Count} capability(ies)[/]");
    }

    private static string FormatStatus(string? status)
    {
        var value = string.IsNullOrWhiteSpace(status) ? "Unknown" : status;
        var color = value.ToLowerInvariant() switch
        {
            "registered" or "available" or "enabled" or "running" or "completed" or "succeeded" or "success" => "green",
            "unknown" or "pending" or "starting" or "stopping" or "unavailable" => "yellow",
            "disabled" or "unregistered" or "failed" or "error" or "cancelled" or "canceled" => "red",
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
