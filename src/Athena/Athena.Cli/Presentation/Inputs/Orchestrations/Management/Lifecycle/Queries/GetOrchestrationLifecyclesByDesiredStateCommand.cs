using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Lifecycle.Queries;

public class GetOrchestrationLifecyclesByDesiredStateCommand(OrchestrationManagementService service)
    : AsyncCommand<GetOrchestrationLifecyclesByDesiredStateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<DESIRED_STATE>")]
        public string DesiredState { get; init; } = string.Empty;

        public override ValidationResult Validate() =>
            string.IsNullOrWhiteSpace(DesiredState) ? ValidationResult.Error("Desired state cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var lifecycles = await service.GetLifecyclesByDesiredStateAsync(settings.DesiredState, cancellationToken);
        OrchestrationManagementOutput.WriteLifecycles(lifecycles);
        return 0;
    }
}
