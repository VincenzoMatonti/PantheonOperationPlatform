using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Commands;

public class CancelOrchestrationLifecycleOperationCommand(OrchestrationManagementService service) : AsyncCommand<CancelOrchestrationLifecycleOperationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<OPERATION_ID>")]
        public Guid OperationId { get; init; }

        public override ValidationResult Validate() =>
            OperationId == Guid.Empty ? ValidationResult.Error("Operation ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operation = await service.CancelOperationAsync(settings.OperationId, cancellationToken);
        OrchestrationManagementOutput.WriteOperation(operation);
        return 0;
    }
}
