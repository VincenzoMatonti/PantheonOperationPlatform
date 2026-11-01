using Athena.Cli.Application.Services.Blocks;
using Athena.Cli.Presentation.Inputs.Systems.Instances.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Blocks;

public sealed class BlockManagementCommand(BlockManagementService service) : AsyncCommand<BlockManagementCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandOption("--action <ACTION>")]
        public string Action { get; init; } = string.Empty;
        [CommandOption("--id <ID>")]
        public Guid Id { get; init; }
        [CommandOption("--state <STATE>")]
        public string? State { get; init; }
        [CommandOption("--operation <TYPE>")]
        public string? Operation { get; init; }
        public override ValidationResult Validate()
        {
            if (string.IsNullOrWhiteSpace(Action)) return ValidationResult.Error("Action is required.");
            if (Action.Equals("list", StringComparison.OrdinalIgnoreCase)) return ValidationResult.Success();
            if (Id == Guid.Empty) return ValidationResult.Error("Block ID cannot be empty.");
            if (Action.Equals("desired", StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(State))
                return ValidationResult.Error("--state is required for the desired action.");
            if (Action.Equals("execute", StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(Operation))
                return ValidationResult.Error("--operation is required for the execute action.");
            return ValidationResult.Success();
        }
    }
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken token)
    {
        object result = settings.Action.ToLowerInvariant() switch
        {
            "list" => await service.GetAllAsync(token),
            "get" => await service.GetAsync(settings.Id, token) ?? throw new InvalidOperationException($"Block lifecycle '{settings.Id}' not found."),
            "create" => await service.CreateAsync(settings.Id, token),
            "desired" => await service.SetDesiredStateAsync(settings.Id, settings.State!, token),
            "execute" => await service.ExecuteAsync(settings.Id, settings.Operation!, token),
            "operations" => await service.GetOperationsAsync(settings.Id, token),
            _ => throw new ArgumentException($"Unsupported block management action '{settings.Action}'.")
        };
        SystemInstanceCommandOutput.Write(result);
        return 0;
    }
}
