using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.RuntimeInstances.Commands;

public class ListRuntimeInstancesCommand(SystemRuntimeInstancesService service) : AsyncCommand<ListRuntimeInstancesCommand.Settings>
{
    public class Settings : CommandSettings;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var instances = await service.GetAllAsync(cancellationToken);
        SystemRuntimeInstanceOutput.RenderAll(instances);
        return 0;
    }
}
