using Athena.Cli.Application.Contracts.Orchestrations;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Orchestrations;

public static class OrchestrationSystemManagementOutput
{
    public static void Write(OrchestrationSystemResponse association)
    {
        var table = new Table().Border(TableBorder.Rounded).Title("[bold]Orchestration System Association[/]").AddColumn("Property").AddColumn("Value");
        table.AddRow("Association ID", association.Id.ToString());
        table.AddRow("Orchestration ID", association.OrchestrationId.ToString());
        table.AddRow("System ID", association.SystemId.ToString());
        table.AddRow("Created At (UTC)", association.CreatedAtUtc.ToString("O"));
        table.AddRow("Updated At (UTC)", association.UpdatedAtUtc?.ToString("O") ?? "-");
        table.AddRow("Deleted", association.IsDeleted ? "Yes" : "No");

        AnsiConsole.Write(table);
    }

    public static void WriteMany(IEnumerable<OrchestrationSystemResponse> associations)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Orchestration System Associations[/]")
            .AddColumn("Association ID")
            .AddColumn("Orchestration ID")
            .AddColumn("System ID")
            .AddColumn("Created At (UTC)")
            .AddColumn("Deleted");

        foreach (var association in associations)
        {
            table.AddRow(
                association.Id.ToString(),
                association.OrchestrationId.ToString(),
                association.SystemId.ToString(),
                association.CreatedAtUtc.ToString("O"),
                association.IsDeleted ? "Yes" : "No");
        }

        AnsiConsole.Write(table);
    }

    public static void WriteMessage(string message)
    {
        AnsiConsole.MarkupLine($"[green]{Markup.Escape(message)}[/]");
    }
}
