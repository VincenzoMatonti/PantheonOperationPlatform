using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Queries;

public class GetOrchestrationLifecycleOperationCommand(OrchestrationManagementService service) : AsyncCommand<GetOrchestrationLifecycleOperationCommand.Settings>
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
        var operation = await service.GetOperationAsync(settings.OperationId, cancellationToken);
        OrchestrationManagementOutput.WriteOperation(operation);
        return 0;
    }
}
