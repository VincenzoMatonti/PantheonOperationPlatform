using Athena.Cli.Application.Contracts.Systems;
using Spectre.Console;

namespace Athena.Cli.Presentation.Outputs.Systems;

public static class SystemManagementOutput
{
    public static void RenderLifecycle(SystemLifecycleResponse lifecycle)
    {
        var table = new Table().Border(TableBorder.Rounded).AddColumn("Property").AddColumn("Value");
        table.AddRow("System ID", lifecycle.SystemId.ToString());
        table.AddRow("Desired state", lifecycle.DesiredState);
        table.AddRow("Operational status", lifecycle.OperationalStatus);
        table.AddRow("Created at (UTC)", lifecycle.CreatedAtUtc.ToString("O"));
        table.AddRow("Updated at (UTC)", lifecycle.UpdatedAtUtc?.ToString("O") ?? "-");
        table.AddRow("Deleted", lifecycle.IsDeleted ? "Yes" : "No");
        AnsiConsole.Write(table);
    }

    public static void RenderLifecycles(IReadOnlyList<SystemLifecycleResponse> lifecycles)
    {
        if (lifecycles.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No lifecycles found.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("System ID")
            .AddColumn("Desired state")
            .AddColumn("Operational status")
            .AddColumn("Created at (UTC)")
            .AddColumn("Deleted");

        foreach (var lifecycle in lifecycles)
        {
            table.AddRow(
                lifecycle.SystemId.ToString(),
                lifecycle.DesiredState,
                lifecycle.OperationalStatus,
                lifecycle.CreatedAtUtc.ToString("yyyy-MM-dd HH:mm:ss"),
                lifecycle.IsDeleted ? "Yes" : "No");
        }

        AnsiConsole.Write(table);
    }

    public static void RenderOperation(SystemLifecycleOperationResponse operation)
    {
        var table = new Table().Border(TableBorder.Rounded).AddColumn("Property").AddColumn("Value");
        table.AddRow("Operation ID", operation.Id.ToString());
        table.AddRow("System ID", operation.SystemId.ToString());
        table.AddRow("Type", operation.Type);
        table.AddRow("Status", operation.Status);
        table.AddRow("Created at (UTC)", operation.CreatedAtUtc.ToString("O"));
        table.AddRow("Updated at (UTC)", operation.UpdatedAtUtc?.ToString("O") ?? "-");
        table.AddRow("Started at (UTC)", operation.StartedAtUtc?.ToString("O") ?? "-");
        table.AddRow("Completed at (UTC)", operation.CompletedAtUtc?.ToString("O") ?? "-");
        table.AddRow("Error", string.IsNullOrWhiteSpace(operation.ErrorMessage) ? "-" : operation.ErrorMessage);
        table.AddRow("Deleted", operation.IsDeleted ? "Yes" : "No");

        AnsiConsole.Write(table);
    }

    public static void RenderOperations(IReadOnlyList<SystemLifecycleOperationResponse> operations)
    {
        if (operations.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No lifecycle operations found.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Operation ID")
            .AddColumn("System ID")
            .AddColumn("Type")
            .AddColumn("Status")
            .AddColumn("Created at (UTC)")
            .AddColumn("Deleted");

        foreach (var operation in operations)
        {
            table.AddRow(
                operation.Id.ToString(),
                operation.SystemId.ToString(),
                operation.Type,
                operation.Status,
                operation.CreatedAtUtc.ToString("yyyy-MM-dd HH:mm:ss"),
                operation.IsDeleted ? "Yes" : "No");
        }

        AnsiConsole.Write(table);
    }

    public static void RenderSuccess(string message)
    {
        AnsiConsole.MarkupLine($"[green]{Markup.Escape(message)}[/]");
    }
}
