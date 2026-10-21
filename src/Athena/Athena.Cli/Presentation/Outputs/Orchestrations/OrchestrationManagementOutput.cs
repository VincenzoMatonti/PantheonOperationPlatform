using Athena.Cli.Application.Contracts.Orchestrations;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Orchestrations;

public static class OrchestrationManagementOutput
{
    public static void WriteLifecycle(OrchestrationLifecycleResponse lifecycle)
    {
        var table = new Table().Border(TableBorder.Rounded).Title("[bold]Orchestration Lifecycle[/]").AddColumn("Property").AddColumn("Value");
        table.AddRow("Orchestration ID", lifecycle.OrchestrationId.ToString());
        table.AddRow("Desired State", lifecycle.DesiredState);
        table.AddRow("Operational Status", lifecycle.OperationalStatus);
        table.AddRow("Created At (UTC)", lifecycle.CreatedAtUtc.ToString("O"));
        table.AddRow("Updated At (UTC)", lifecycle.UpdatedAtUtc?.ToString("O") ?? "-");
        table.AddRow("Deleted", lifecycle.IsDeleted ? "Yes" : "No");
        AnsiConsole.Write(table);
    }

    public static void WriteLifecycles(IEnumerable<OrchestrationLifecycleResponse> lifecycles)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Orchestration Lifecycles[/]")
            .AddColumn("Orchestration ID")
            .AddColumn("Desired State")
            .AddColumn("Operational Status")
            .AddColumn("Created At (UTC)")
            .AddColumn("Deleted");

        foreach (var lifecycle in lifecycles)
        {
            table.AddRow(
                lifecycle.OrchestrationId.ToString(),
                lifecycle.DesiredState,
                lifecycle.OperationalStatus,
                lifecycle.CreatedAtUtc.ToString("O"),
                lifecycle.IsDeleted ? "Yes" : "No");
        }

        AnsiConsole.Write(table);
    }

    public static void WriteOperation(OrchestrationLifecycleOperationResponse operation)
    {
        var table = new Table().Border(TableBorder.Rounded).Title("[bold]Orchestration Lifecycle Operation[/]").AddColumn("Property").AddColumn("Value");
        table.AddRow("Operation ID", operation.Id.ToString());
        table.AddRow("Orchestration ID", operation.OrchestrationId.ToString());
        table.AddRow("Type", operation.Type);
        table.AddRow("Status", operation.Status);
        table.AddRow("Created At (UTC)", operation.CreatedAtUtc.ToString("O"));
        table.AddRow("Updated At (UTC)", operation.UpdatedAtUtc?.ToString("O") ?? "-");
        table.AddRow("Started At (UTC)", operation.StartedAtUtc?.ToString("O") ?? "-");
        table.AddRow("Completed At (UTC)", operation.CompletedAtUtc?.ToString("O") ?? "-");
        table.AddRow("Error", operation.ErrorMessage ?? "-");
        table.AddRow("Deleted", operation.IsDeleted ? "Yes" : "No");

        AnsiConsole.Write(table);
    }

    public static void WriteOperations(IEnumerable<OrchestrationLifecycleOperationResponse> operations)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Orchestration Lifecycle Operations[/]")
            .AddColumn("Operation ID")
            .AddColumn("Orchestration ID")
            .AddColumn("Type")
            .AddColumn("Status")
            .AddColumn("Created At (UTC)")
            .AddColumn("Deleted");

        foreach (var operation in operations)
        {
            table.AddRow(
                operation.Id.ToString(),
                operation.OrchestrationId.ToString(),
                operation.Type,
                operation.Status,
                operation.CreatedAtUtc.ToString("O"),
                operation.IsDeleted ? "Yes" : "No");
        }

        AnsiConsole.Write(table);
    }

    public static void WriteMessage(string message)
    {
        AnsiConsole.MarkupLine($"[green]{Markup.Escape(message)}[/]");
    }

    public static void WriteError(string message)
    {
        AnsiConsole.MarkupLine($"[red]Error: {Markup.Escape(message)}[/]");
    }
}
