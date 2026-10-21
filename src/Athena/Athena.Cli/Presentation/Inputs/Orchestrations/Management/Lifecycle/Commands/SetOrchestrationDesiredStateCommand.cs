
using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Lifecycle.Commands;

public class SetOrchestrationDesiredStateCommand(OrchestrationManagementService service) : AsyncCommand<SetOrchestrationDesiredStateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ORCHESTRATION_ID>")]
        public Guid OrchestrationId { get; init; }

        [CommandArgument(1, "<DESIRED_STATE>")]
        public string DesiredState { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (OrchestrationId == Guid.Empty) return ValidationResult.Error("Orchestration ID cannot be empty.");
            return string.IsNullOrWhiteSpace(DesiredState) ? ValidationResult.Error("Desired state cannot be empty.") : ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var lifecycle = await service.SetDesiredStateAsync(settings.OrchestrationId, new SetOrchestrationDesiredStateRequest(settings.DesiredState), cancellationToken);
        OrchestrationManagementOutput.WriteLifecycle(lifecycle);
        return 0;
    }
}
