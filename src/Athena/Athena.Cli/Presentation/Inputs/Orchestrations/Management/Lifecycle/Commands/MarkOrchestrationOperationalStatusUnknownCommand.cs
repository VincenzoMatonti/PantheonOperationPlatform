using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Lifecycle.Commands;

public class MarkOrchestrationOperationalStatusUnknownCommand(OrchestrationManagementService service) : AsyncCommand<MarkOrchestrationOperationalStatusUnknownCommand.Settings>
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
        var lifecycle = await service.MarkOperationalStatusUnknownAsync(settings.OrchestrationId, cancellationToken);
        OrchestrationManagementOutput.WriteLifecycle(lifecycle);
        return 0;
    }
}
