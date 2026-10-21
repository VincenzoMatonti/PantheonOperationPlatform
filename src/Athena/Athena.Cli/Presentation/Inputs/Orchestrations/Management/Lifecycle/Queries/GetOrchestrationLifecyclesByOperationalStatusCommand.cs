using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Lifecycle.Queries;

public class GetOrchestrationLifecyclesByOperationalStatusCommand(OrchestrationManagementService service)    : AsyncCommand<GetOrchestrationLifecyclesByOperationalStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<OPERATIONAL_STATUS>")]
        public string OperationalStatus { get; init; } = string.Empty;

        public override ValidationResult Validate() =>
            string.IsNullOrWhiteSpace(OperationalStatus) ? ValidationResult.Error("Operational status cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var lifecycles = await service.GetLifecyclesByOperationalStatusAsync(settings.OperationalStatus, cancellationToken);
        OrchestrationManagementOutput.WriteLifecycles(lifecycles);
        return 0;
    }
}
