using Athena.Cli.Application.Contracts.Systems;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Systems;

public static class SystemRegistryOutput
{
    public static void WriteSystems(IEnumerable<SystemResponse> systems)
    {
        var items = systems.ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No systems found.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Systems[/]")
            .AddColumn("ID")
            .AddColumn("Name")
            .AddColumn("Registration")
            .AddColumn("Availability");

        foreach (var system in items)
        {
            table.AddRow(
                system.Id.ToString(),
                Markup.Escape(system.Name),
                Markup.Escape(system.RegistrationStatus),
                Markup.Escape(system.AvailabilityStatus));
        }

        AnsiConsole.Write(table);
    }

    public static void WriteSystem(SystemResponse system)
    {
        var details = new Table().Border(TableBorder.Simple).HideHeaders().AddColumn("Property").AddColumn("Value");
        details.AddRow("ID", system.Id.ToString());
        details.AddRow("Name", Markup.Escape(system.Name));
        details.AddRow("Registration", Markup.Escape(system.RegistrationStatus));
        details.AddRow("Availability", Markup.Escape(system.AvailabilityStatus));
        details.AddRow("Created (UTC)", system.CreatedAtUtc.ToString("O"));
        details.AddRow("Updated (UTC)", system.UpdatedAtUtc?.ToString("O") ?? "-");
        details.AddRow("Deleted", system.IsDeleted.ToString());
        AnsiConsole.Write(new Panel(details).Header("[bold]System details[/]").Border(BoxBorder.Rounded));
        var endpoints = new Table().Border(TableBorder.Rounded).Title("[bold]Endpoints[/]").AddColumn("Name").AddColumn("Base address");
        foreach (var endpoint in system.Endpoints)
        {
            endpoints.AddRow(Markup.Escape(endpoint.Name), Markup.Escape(endpoint.BaseAddress));
        }

        if (system.Endpoints.Count == 0)
        {
            AnsiConsole.MarkupLine("[dim]No endpoints.[/]");
        }
        else
        {
            AnsiConsole.Write(endpoints);
        }

        var capabilities = new Table().Border(TableBorder.Rounded).Title("[bold]Capabilities[/]").AddColumn("Name").AddColumn("Description");
        foreach (var capability in system.Capabilities)
        {
            capabilities.AddRow(Markup.Escape(capability.Name), Markup.Escape(capability.Description));
        }

        if (system.Capabilities.Count == 0)
        {
            AnsiConsole.MarkupLine("[dim]No capabilities.[/]");
        }
        else
        {
            AnsiConsole.Write(capabilities);
        }
    }

    public static void WriteMessage(string message)
    {
        AnsiConsole.MarkupLine($"[green]{Markup.Escape(message)}[/]");
    }

    public static void WriteError(string message)
    {
        AnsiConsole.MarkupLine($"[red]Error:[/] {Markup.Escape(message)}");
    }

    public static void WriteEndpoints(IEnumerable<SystemEndpointResponse> endpoints)
    {
        var items = endpoints.ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No endpoints found.[/]");
            return;
        }

        var table = new Table().Border(TableBorder.Rounded).AddColumn("Name").AddColumn("Base address");
        foreach (var endpoint in items)
        {
            table.AddRow(Markup.Escape(endpoint.Name), Markup.Escape(endpoint.BaseAddress));
        }

        AnsiConsole.Write(table);
    }

    public static void WriteCapabilities(IEnumerable<SystemCapabilityResponse> capabilities)
    {
        var items = capabilities.ToList();
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No capabilities found.[/]");
            return;
        }

        var table = new Table().Border(TableBorder.Rounded).AddColumn("Name").AddColumn("Description");
        foreach (var capability in items)
        {
            table.AddRow(Markup.Escape(capability.Name), Markup.Escape(capability.Description));
        }

        AnsiConsole.Write(table);
    }
}
