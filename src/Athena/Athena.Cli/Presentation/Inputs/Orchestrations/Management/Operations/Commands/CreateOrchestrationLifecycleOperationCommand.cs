using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Commands;

public class CreateOrchestrationLifecycleOperationCommand(OrchestrationManagementService service) : AsyncCommand<CreateOrchestrationLifecycleOperationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ORCHESTRATION_ID>")]
        public Guid OrchestrationId { get; init; }

        [CommandArgument(1, "<OPERATION_ID>")]
        public Guid OperationId { get; init; }

        [CommandArgument(2, "<TYPE>")]
        public string Type { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (OrchestrationId == Guid.Empty) return ValidationResult.Error("Orchestration ID cannot be empty.");
            if (OperationId == Guid.Empty) return ValidationResult.Error("Operation ID cannot be empty.");
            return string.IsNullOrWhiteSpace(Type) ? ValidationResult.Error("Operation type cannot be empty.") : ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operation = await service.CreateOperationAsync(settings.OrchestrationId, new CreateOrchestrationLifecycleOperationRequest(settings.OperationId, settings.Type), cancellationToken);
        OrchestrationManagementOutput.WriteOperation(operation);
        return 0;
    }
}
