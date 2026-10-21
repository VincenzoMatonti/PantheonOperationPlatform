using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Queries;

public class ListDeletedSystemLifecyclesCommand(SystemManagementService service) : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var lifecycles = await service.GetDeletedLifecyclesAsync(cancellationToken);
        SystemManagementOutput.RenderLifecycles(lifecycles);
        return 0;
    }
}
