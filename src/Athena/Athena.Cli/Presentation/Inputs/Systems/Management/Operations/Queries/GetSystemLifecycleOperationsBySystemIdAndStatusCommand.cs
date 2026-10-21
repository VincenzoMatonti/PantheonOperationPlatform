using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Queries;

public class GetSystemLifecycleOperationsBySystemIdAndStatusCommand(SystemManagementService service)
  : AsyncCommand<GetSystemLifecycleOperationsBySystemIdAndStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        [CommandArgument(1, "<STATUS>")]
        public string Status { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (SystemId == Guid.Empty) return ValidationResult.Error("System ID cannot be empty.");
            return string.IsNullOrWhiteSpace(Status) ? ValidationResult.Error("Operation status is required.") : ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operations = await service.GetOperationsBySystemIdAndStatusAsync(settings.SystemId, settings.Status, cancellationToken);
        SystemManagementOutput.RenderOperations(operations);
        return 0;
    }
}
