using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Operations.Queries;

public class GetFailedOrchestrationLifecycleOperationsCommand(OrchestrationManagementService service)
    : AsyncCommand<GetFailedOrchestrationLifecycleOperationsCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operations = await service.GetFailedOperationsAsync(cancellationToken);
        OrchestrationManagementOutput.WriteOperations(operations);
        return 0;
    }
}
