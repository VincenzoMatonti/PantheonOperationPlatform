using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Commands;

public class CreateSystemLifecycleOperationCommand(SystemManagementService service) : AsyncCommand<CreateSystemLifecycleOperationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        [CommandArgument(1, "<TYPE>")]
        public string Type { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (SystemId == Guid.Empty) return ValidationResult.Error("System ID cannot be empty.");
            return string.IsNullOrWhiteSpace(Type) ? ValidationResult.Error("Operation type is required.") : ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operation = await service.CreateOperationAsync(settings.SystemId, new CreateSystemLifecycleOperationRequest(settings.Type), cancellationToken);
        SystemManagementOutput.RenderOperation(operation);
        return 0;
    }
}
