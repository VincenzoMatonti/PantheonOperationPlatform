using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Commands;

public class ExecuteOrchestrationRuntimeOperationCommand(OrchestrationManagementService service)
    : AsyncCommand<ExecuteOrchestrationRuntimeOperationCommand.Settings>
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
            if (string.IsNullOrWhiteSpace(Type)) return ValidationResult.Error("Operation type cannot be empty.");
            if (!new[] { "Start", "Stop", "Restart" }.Contains(Type, StringComparer.OrdinalIgnoreCase))
                return ValidationResult.Error("Operation type must be Start, Stop, or Restart.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var request = new CreateOrchestrationLifecycleOperationRequest(settings.OperationId, settings.Type);
        var operation = await service.ExecuteRuntimeOperationAsync(settings.OrchestrationId, request, cancellationToken);
        OrchestrationManagementOutput.WriteOperation(operation);
        return 0;
    }
}