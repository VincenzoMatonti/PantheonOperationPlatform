using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Commands;

public class FailSystemLifecycleOperationCommand(SystemManagementService service) : AsyncCommand<FailSystemLifecycleOperationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<OPERATION_ID>")]
        public Guid OperationId { get; init; }

        [CommandOption("--error <MESSAGE>")]
        public string ErrorMessage { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (OperationId == Guid.Empty) return ValidationResult.Error("Operation ID cannot be empty.");
            return string.IsNullOrWhiteSpace(ErrorMessage) ? ValidationResult.Error("An error message is required.") : ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operation = await service.FailOperationAsync(settings.OperationId, new FailSystemLifecycleOperationRequest(settings.ErrorMessage), cancellationToken);
        SystemManagementOutput.RenderOperation(operation);
        return 0;
    }
}
