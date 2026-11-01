using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.OrchestrationSystemInstances.Queries;

public class ListOrchestrationSystemInstancesCommand(OrchestrationSystemInstanceManagementService service) : AsyncCommand<ListOrchestrationSystemInstancesCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--include-deleted")]
        public bool IncludeDeleted { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var associations = await service.GetAllAsync(settings.IncludeDeleted, cancellationToken);
        OrchestrationSystemInstanceManagementOutput.WriteMany(associations);

        return 0;
    }
}