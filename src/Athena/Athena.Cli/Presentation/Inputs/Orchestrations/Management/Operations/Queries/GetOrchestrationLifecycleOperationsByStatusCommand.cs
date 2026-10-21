using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Queries;

public class GetOrchestrationLifecycleOperationsByStatusCommand(OrchestrationManagementService service)
    : AsyncCommand<GetOrchestrationLifecycleOperationsByStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<STATUS>")]
        public string Status { get; init; } = string.Empty;

        public override ValidationResult Validate() =>
            string.IsNullOrWhiteSpace(Status) ? ValidationResult.Error("Operation status cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operations = await service.GetOperationsByStatusAsync(settings.Status, cancellationToken);
        OrchestrationManagementOutput.WriteOperations(operations);
        return 0;
    }
}
