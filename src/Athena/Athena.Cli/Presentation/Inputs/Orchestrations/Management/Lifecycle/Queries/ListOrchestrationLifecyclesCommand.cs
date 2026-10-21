using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.Lifecycle.Queries;

public class ListOrchestrationLifecyclesCommand(OrchestrationManagementService service) : AsyncCommand<ListOrchestrationLifecyclesCommand.Settings>
{
    public class Settings : CommandSettings;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var lifecycles = await service.GetAllLifecyclesAsync(cancellationToken);
        OrchestrationManagementOutput.WriteLifecycles(lifecycles);
        return 0;
    }
}
