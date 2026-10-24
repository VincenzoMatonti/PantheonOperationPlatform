using Athena.Cli.Application.Contracts.Systems;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Systems;

public static class SystemRuntimeInstanceOutput
{
    public static void RenderAll(IReadOnlyList<SystemRuntimeInstanceResponse> instances)
    {
        RenderHeader("RUNTIME INSTANCES", "Registered runtime resources across all systems");
        if (instances.Count == 0)
        {
            RenderEmpty("No runtime instances are registered in Athena.");
            return;
        }

        var table = CreateTable();
        foreach (var instance in instances.OrderBy(x => x.SystemId)
                     .ThenBy(x => x.Environment)
                     .ThenBy(x => x.RuntimeResourceName))
        {
            AddInstanceRow(table, instance);
        }

        AnsiConsole.Write(table);
        RenderSummary(instances);
    }

    public static void RenderDetails(SystemRuntimeInstanceResponse instance)
    {
        RenderHeader("RUNTIME INSTANCE DETAILS", instance.RuntimeResourceName);
        var grid = new Grid().AddColumn(new GridColumn().NoWrap()).AddColumn();
        grid.AddRow(new Markup("[bold]Instance ID[/]"), Value(instance.Id.ToString()));
        grid.AddRow(new Markup("[bold]System ID[/]"), Value(instance.SystemId.ToString()));
        grid.AddRow(new Markup("[bold]Resource name[/]"), Value(instance.RuntimeResourceName));
        grid.AddRow(new Markup("[bold]Resource ID[/]"), Value(instance.RuntimeResourceId));
        grid.AddRow(new Markup("[bold]Provider[/]"), Value(instance.RuntimeProvider));
        grid.AddRow(new Markup("[bold]Environment[/]"), EnvironmentValue(instance.Environment));
        grid.AddRow(new Markup("[bold]Created at (UTC)[/]"), Value(FormatUtc(instance.CreatedAtUtc)));
        grid.AddRow(new Markup("[bold]Updated at (UTC)[/]"), Value(FormatUtc(instance.UpdatedAtUtc)));
        var panel = new Panel(grid).Header("[bold]Runtime Instance[/]").Border(BoxBorder.Rounded).BorderColor(Color.Cyan).Padding(1, 0);
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
        RenderCopyableValue("Instance ID", instance.Id.ToString());
        RenderCopyableValue("System ID", instance.SystemId.ToString());
    }

    public static void RenderBySystem(Guid systemId, IReadOnlyList<SystemRuntimeInstanceResponse> instances)
    {
        RenderHeader("SYSTEM RUNTIME INSTANCES", $"System: {systemId}");
        if (instances.Count == 0)
        {
            RenderEmpty($"No runtime instances found for system {systemId}.");
            return;
        }

        var table = CreateTable();
        foreach (var instance in instances.OrderBy(x => x.Environment)
                     .ThenBy(x => x.RuntimeResourceName))
        {
            AddInstanceRow(table, instance);
        }

        AnsiConsole.Write(table);
        RenderSummary(instances);
    }

    public static void RenderBySystemAndEnvironment(Guid systemId, string environment, IReadOnlyList<SystemRuntimeInstanceResponse> instances)
    {
        RenderHeader("RUNTIME INSTANCES BY ENVIRONMENT", $"System: {systemId} | Environment: {DisplayValue(environment)}");
        if (instances.Count == 0)
        {
            RenderEmpty("No runtime instances match the requested system and environment.");
            return;
        }

        var table = CreateTable();
        foreach (var instance in instances.OrderBy(x => x.RuntimeResourceName))
        {
            AddInstanceRow(table, instance);
        }

        AnsiConsole.Write(table);
        RenderSummary(instances);
    }

    public static void RenderByResourceName(string runtimeResourceName, IReadOnlyList<SystemRuntimeInstanceResponse> instances)
    {
        RenderHeader("RUNTIME RESOURCE SEARCH", $"Name: {runtimeResourceName}");
        if (instances.Count == 0)
        {
            RenderEmpty($"No runtime instances found with resource name '{runtimeResourceName}'.");
            return;
        }

        var table = CreateTable();
        foreach (var instance in instances.OrderBy(x => x.SystemId).ThenBy(x => x.Environment))
        {
            AddInstanceRow(table, instance);
        }

        AnsiConsole.Write(table);
        RenderSummary(instances);
    }

    public static void RenderRegistered(SystemRuntimeInstanceResponse instance)
    {
        AnsiConsole.Write(new Rule("[bold green]RUNTIME INSTANCE REGISTERED[/]").RuleStyle(Color.Green));
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[green bold]✔ Runtime instance registered successfully.[/]");
        AnsiConsole.WriteLine();
        RenderDetails(instance);
    }

    public static void RenderUnregistered(Guid id)
    {
        AnsiConsole.Write(new Rule("[bold yellow]RUNTIME INSTANCE UNREGISTERED[/]").RuleStyle(Color.Yellow));
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[green bold]✔ Runtime instance registration removed successfully.[/]");
        AnsiConsole.WriteLine();
        var panel = new Panel(new Markup($"[bold]Instance ID:[/] {Markup.Escape(id.ToString())}\n" + "[grey]The runtime resource itself was not necessarily stopped or deleted.[/]"))
            .Header("[bold]Unregistration Summary[/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Yellow)
            .Padding(1, 0);
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    public static void RenderNotFound(string id)
    {
        AnsiConsole.Write(new Rule("[bold yellow]RUNTIME INSTANCE NOT FOUND[/]").RuleStyle(Color.Yellow));
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[yellow]No runtime instance was found for ID {Markup.Escape(id)}.[/]");
        AnsiConsole.WriteLine();
    }

    public static void RenderSuccess(string message)
    {
        AnsiConsole.MarkupLine($"[green bold]✔ {Markup.Escape(message)}[/]");
        AnsiConsole.WriteLine();
    }

    public static void RenderError(string message)
    {
        AnsiConsole.MarkupLine($"[red bold]✖ {Markup.Escape(message)}[/]");
        AnsiConsole.WriteLine();
    }


    private static Table CreateTable()
    {
        return new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Cyan1)
            .Title("[bold cyan]Registered Runtime Resources[/]")
            .AddColumn(new TableColumn("[bold cyan]Instance ID[/]"))
            .AddColumn(new TableColumn("[bold cyan]Resource Name[/]"))
            .AddColumn(new TableColumn("[bold cyan]Provider[/]"))
            .AddColumn(new TableColumn("[bold cyan]Environment[/]").Centered())
            .AddColumn(new TableColumn("[bold cyan]System ID[/]"))
            .AddColumn(new TableColumn("[bold cyan]Resource ID[/]"))
            .AddColumn(new TableColumn("[bold cyan]Updated (UTC)[/]").NoWrap());
    }

    private static void AddInstanceRow(Table table, SystemRuntimeInstanceResponse instance)
    {
        table.AddRow(
            new Markup($"[dim]{Markup.Escape(instance.Id.ToString())}[/]"),
            new Markup($"[bold white]{Markup.Escape(DisplayValue(instance.RuntimeResourceName))}[/]"),
            new Markup($"[cyan]{Markup.Escape(DisplayValue(instance.RuntimeProvider))}[/]"),
            EnvironmentValue(instance.Environment),
            new Markup($"[dim]{Markup.Escape(ShortId(instance.SystemId))}[/]"),
            new Markup($"[grey]{Markup.Escape(DisplayValue(instance.RuntimeResourceId))}[/]"),
            new Markup($"[dim]{Markup.Escape(FormatUtc(instance.UpdatedAtUtc))}[/]"));
    }

    private static void RenderSummary(IReadOnlyList<SystemRuntimeInstanceResponse> instances)
    {
        var systemCount = instances.Select(x => x.SystemId).Distinct().Count();
        var providerCount = instances.Select(x => x.RuntimeProvider).Distinct(StringComparer.OrdinalIgnoreCase).Count();
        var environments = instances.GroupBy(x => DisplayValue(x.Environment), StringComparer.OrdinalIgnoreCase).OrderBy(x => x.Key).Select(x => $"{x.Key}: {x.Count()}");
        var summary = new Grid().AddColumn().AddColumn().AddColumn();
        summary.AddRow(Metric("RUNTIME INSTANCES", instances.Count.ToString()), Metric("REGISTERED SYSTEMS", systemCount.ToString()), Metric("RUNTIME PROVIDERS", providerCount.ToString()));
        AnsiConsole.Write(new Panel(summary).Header("[bold cyan]INVENTORY SUMMARY[/]").Border(BoxBorder.Rounded).BorderColor(Color.Cyan1).Padding(1, 0));
        AnsiConsole.MarkupLine($"[bold cyan]ENVIRONMENTS[/]  [grey]{Markup.Escape(string.Join("  │  ", environments))}[/]");
        AnsiConsole.WriteLine();
    }


    private static void RenderHeader(string title, string subtitle)
    {
        AnsiConsole.Write(new Rule($"[bold cyan]{Markup.Escape(title)}[/]").RuleStyle(Color.Cyan));
        AnsiConsole.MarkupLine($"[grey]{Markup.Escape(subtitle)}[/]");
        AnsiConsole.WriteLine();
    }

    private static void RenderEmpty(string message)
    {
        AnsiConsole.Write(new Panel(new Markup($"[yellow]{Markup.Escape(message)}[/]"))
                   .Border(BoxBorder.Rounded).BorderColor(Color.Yellow).Padding(1, 0));
        AnsiConsole.WriteLine();
    }

    private static void RenderCopyableValue(string label, string value)
    {
        AnsiConsole.MarkupLine($"[grey]{Markup.Escape(label)}:[/] {Markup.Escape(value)}");
    }

    private static Markup Metric(string label, string value) => new($"[grey]{Markup.Escape(label)}[/]\n[bold cyan]{Markup.Escape(value)}[/]");
    private static Markup Value(string? value) => new(Markup.Escape(DisplayValue(value)));

    private static Markup EnvironmentValue(string? environment)
    {
        var value = DisplayValue(environment);
        var color = value.Trim().ToLowerInvariant() switch
        {
            "production" or "prod" => "green",
            "staging" or "stage" => "yellow",
            "development" or "dev" or "local" => "cyan",
            _ => "white"
        };

        return new Markup($"[{color}]{Markup.Escape(value)}[/]");
    }

    private static string ShortId(Guid id) => id.ToString("N")[..8];
    private static string FormatUtc(DateTime value) => value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
    private static string DisplayValue(string? value) => string.IsNullOrWhiteSpace(value) ? "Not available" : value;
}
