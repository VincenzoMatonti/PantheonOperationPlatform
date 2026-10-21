using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Commands;

public class FailOrchestrationLifecycleOperationCommand(OrchestrationManagementService service) : AsyncCommand<FailOrchestrationLifecycleOperationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<OPERATION_ID>")]
        public Guid OperationId { get; init; }

        [CommandArgument(1, "<ERROR_MESSAGE>")]
        public string ErrorMessage { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (OperationId == Guid.Empty) return ValidationResult.Error("Operation ID cannot be empty.");
            return string.IsNullOrWhiteSpace(ErrorMessage) ? ValidationResult.Error("Error message cannot be empty.") : ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operation = await service.FailOperationAsync(settings.OperationId, new FailOrchestrationLifecycleOperationRequest(settings.ErrorMessage), cancellationToken);
        OrchestrationManagementOutput.WriteOperation(operation);
        return 0;
    }
}
