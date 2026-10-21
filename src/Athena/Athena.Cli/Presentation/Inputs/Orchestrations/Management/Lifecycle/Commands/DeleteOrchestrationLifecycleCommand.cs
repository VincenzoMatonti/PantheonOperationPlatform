using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Lifecycle.Commands;

public class DeleteOrchestrationLifecycleCommand(OrchestrationManagementService service) : AsyncCommand<DeleteOrchestrationLifecycleCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ORCHESTRATION_ID>")]
        public Guid OrchestrationId { get; init; }

        public override ValidationResult Validate() =>
            OrchestrationId == Guid.Empty ? ValidationResult.Error("Orchestration ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        await service.DeleteLifecycleAsync(settings.OrchestrationId, cancellationToken);
        OrchestrationManagementOutput.WriteMessage($"Lifecycle for orchestration '{settings.OrchestrationId}' deleted.");
        return 0;
    }
}
