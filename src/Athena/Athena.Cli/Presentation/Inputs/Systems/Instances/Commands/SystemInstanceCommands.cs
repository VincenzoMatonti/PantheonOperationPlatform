using System.Text.Json;
using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Instances.Commands;

internal static class SystemInstanceCommandOutput
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public static void Write(object value) => AnsiConsole.WriteLine(JsonSerializer.Serialize(value, Options));
}

public sealed class ListSystemInstancesCommand(SystemInstancesService service) : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        SystemInstanceCommandOutput.Write(await service.GetAllAsync(cancellationToken));
        return 0;
    }
}

public sealed class GetSystemInstanceCommand(SystemInstancesService service) : AsyncCommand<GetSystemInstanceCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id <ID>")]
        public Guid Id { get; init; }

        public override ValidationResult Validate() =>
            Id == Guid.Empty ? ValidationResult.Error("SystemInstance ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(settings.Id, cancellationToken);
        if (result is null)
        {
            AnsiConsole.MarkupLine($"[yellow]SystemInstance '{settings.Id}' was not found.[/]");
            return 1;
        }

        SystemInstanceCommandOutput.Write(result);
        return 0;
    }
}

public sealed class CreateSystemInstanceCommand(SystemInstancesService service) : AsyncCommand<CreateSystemInstanceCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--system-id <SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        [CommandOption("--workload-id <WORKLOAD_ID>")]
        public Guid WorkloadId { get; init; }

        [CommandOption("--name <NAME>")]
        public string Name { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (SystemId == Guid.Empty) return ValidationResult.Error("System ID cannot be empty.");
            if (WorkloadId == Guid.Empty) return ValidationResult.Error("Workload ID cannot be empty.");
            if (string.IsNullOrWhiteSpace(Name)) return ValidationResult.Error("Name is required.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var request = new CreateSystemInstanceRequest(Guid.NewGuid(), settings.SystemId, settings.WorkloadId, settings.Name);
        SystemInstanceCommandOutput.Write(await service.CreateAsync(request, cancellationToken));
        return 0;
    }
}

public abstract class SystemInstanceIdCommand(SystemInstancesService service) : AsyncCommand<SystemInstanceIdCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id <ID>")]
        public Guid Id { get; init; }

        public override ValidationResult Validate() =>
            Id == Guid.Empty ? ValidationResult.Error("SystemInstance ID cannot be empty.") : ValidationResult.Success();
    }

    protected abstract Task<object> ExecuteOperationAsync(Guid id, CancellationToken cancellationToken);

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        SystemInstanceCommandOutput.Write(await ExecuteOperationAsync(settings.Id, cancellationToken));
        return 0;
    }

    protected SystemInstancesService Service { get; } = service;
}

public sealed class RegisterSystemInstanceCommand(SystemInstancesService service) : SystemInstanceIdCommand(service)
{
    protected override Task<object> ExecuteOperationAsync(Guid id, CancellationToken cancellationToken) =>
        WrapAsync(Service.RegisterAsync(new RegisterSystemInstanceRequest(id), cancellationToken));

    private static async Task<object> WrapAsync(Task<SystemInstanceStatusResponse> operation) => await operation;
}

public sealed class StartSystemInstanceCommand(SystemInstancesService service) : SystemInstanceIdCommand(service)
{
    protected override Task<object> ExecuteOperationAsync(Guid id, CancellationToken cancellationToken) =>
        WrapAsync(Service.StartAsync(id, cancellationToken));

    private static async Task<object> WrapAsync(Task<SystemInstanceOperationResponse> operation) => await operation;
}

public sealed class StopSystemInstanceCommand(SystemInstancesService service) : SystemInstanceIdCommand(service)
{
    protected override Task<object> ExecuteOperationAsync(Guid id, CancellationToken cancellationToken) =>
        WrapAsync(Service.StopAsync(id, cancellationToken));

    private static async Task<object> WrapAsync(Task<SystemInstanceOperationResponse> operation) => await operation;
}

public sealed class RestartSystemInstanceCommand(SystemInstancesService service) : SystemInstanceIdCommand(service)
{
    protected override Task<object> ExecuteOperationAsync(Guid id, CancellationToken cancellationToken) =>
        WrapAsync(Service.RestartAsync(id, cancellationToken));

    private static async Task<object> WrapAsync(Task<SystemInstanceOperationResponse> operation) => await operation;
}

public sealed class RemoveSystemInstanceCommand(SystemInstancesService service) : SystemInstanceIdCommand(service)
{
    protected override Task<object> ExecuteOperationAsync(Guid id, CancellationToken cancellationToken) =>
        WrapAsync(Service.RemoveAsync(id, cancellationToken));

    private static async Task<object> WrapAsync(Task<SystemInstanceStatusResponse> operation) => await operation;
}

public sealed class GetSystemInstanceStatusCommand(SystemInstancesService service) : SystemInstanceIdCommand(service)
{
    protected override Task<object> ExecuteOperationAsync(Guid id, CancellationToken cancellationToken) =>
        WrapAsync(Service.GetStatusAsync(id, cancellationToken));

    private static async Task<object> WrapAsync(Task<SystemInstanceStatusResponse> operation) => await operation;
}

public sealed class GetSystemInstanceHealthCommand(SystemInstancesService service) : SystemInstanceIdCommand(service)
{
    protected override Task<object> ExecuteOperationAsync(Guid id, CancellationToken cancellationToken) =>
        WrapAsync(Service.GetHealthAsync(id, cancellationToken));

    private static async Task<object> WrapAsync(Task<SystemInstanceHealthResponse> operation) => await operation;
}

public sealed class GetSystemInstanceLogsCommand(SystemInstancesService service) : AsyncCommand<GetSystemInstanceLogsCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id <ID>")]
        public Guid Id { get; init; }

        [CommandOption("--tail <COUNT>")]
        public int Tail { get; init; } = 100;

        public override ValidationResult Validate()
        {
            if (Id == Guid.Empty) return ValidationResult.Error("SystemInstance ID cannot be empty.");
            return Tail is < 1 or > 1000 ? ValidationResult.Error("Tail must be between 1 and 1000.") : ValidationResult.Success();
        }

    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        SystemInstanceCommandOutput.Write(await service.GetLogsAsync(settings.Id, settings.Tail, cancellationToken));
        return 0;
    }
}

public sealed class GetSystemInstanceOperationsCommand(SystemInstancesService service) : AsyncCommand<GetSystemInstanceOperationsCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id <ID>")]
        public Guid Id { get; init; }

        public override ValidationResult Validate() =>
            Id == Guid.Empty ? ValidationResult.Error("SystemInstance ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        SystemInstanceCommandOutput.Write(await service.GetOperationsAsync(settings.Id, cancellationToken));
        return 0;
    }
}

public sealed class GetSystemInstanceOperationCommand(SystemInstancesService service) : AsyncCommand<GetSystemInstanceOperationCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--id <ID>")]
        public Guid Id { get; init; }

        [CommandOption("--operation-id <OPERATION_ID>")]
        public Guid OperationId { get; init; }

        public override ValidationResult Validate()
        {
            if (Id == Guid.Empty) return ValidationResult.Error("SystemInstance ID cannot be empty.");
            return OperationId == Guid.Empty ? ValidationResult.Error("Operation ID cannot be empty.") : ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var result = await service.GetOperationAsync(settings.Id, settings.OperationId, cancellationToken);
        if (result is null)
        {
            AnsiConsole.MarkupLine($"[yellow]Operation '{settings.OperationId}' was not found for SystemInstance '{settings.Id}'.[/]");
            return 1;
        }
        SystemInstanceCommandOutput.Write(result);
        return 0;
    }
}
