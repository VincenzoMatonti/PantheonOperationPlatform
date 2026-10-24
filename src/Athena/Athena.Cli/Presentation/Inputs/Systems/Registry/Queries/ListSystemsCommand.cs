using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;

public class ListSystemsCommand(SystemRegistryService systemRegistryService) : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var systems = await systemRegistryService.GetAllAsync(cancellationToken);
        SystemRegistryOutput.WriteSystems(systems);
        return 0;
    }
}
