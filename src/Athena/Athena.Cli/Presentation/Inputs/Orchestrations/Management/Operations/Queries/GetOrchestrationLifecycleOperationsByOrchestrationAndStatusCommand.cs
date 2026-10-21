using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Queries;

public class GetOrchestrationLifecycleOperationsByOrchestrationAndStatusCommand(OrchestrationManagementService service)
    : AsyncCommand<GetOrchestrationLifecycleOperationsByOrchestrationAndStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ORCHESTRATION_ID>")]
        public Guid OrchestrationId { get; init; }

        [CommandArgument(1, "<STATUS>")]
        public string Status { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (OrchestrationId == Guid.Empty) return ValidationResult.Error("Orchestration ID cannot be empty.");
            return string.IsNullOrWhiteSpace(Status) ? ValidationResult.Error("Operation status cannot be empty.") : ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operations = await service.GetOperationsByOrchestrationIdAndStatusAsync(settings.OrchestrationId, settings.Status, cancellationToken);
        OrchestrationManagementOutput.WriteOperations(operations);
        return 0;
    }
}
