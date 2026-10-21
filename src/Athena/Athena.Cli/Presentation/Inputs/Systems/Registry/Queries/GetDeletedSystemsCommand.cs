using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;

public class GetDeletedSystemsCommand(    SystemRegistryService service)    : AsyncCommand
{
    public override async Task<int> ExecuteAsync(        CommandContext context,        CancellationToken cancellationToken)
    {
        try
        {
            var systems = await service.GetDeletedAsync(cancellationToken);
            SystemRegistryOutput.WriteSystems(systems);
            return 0;
        }
        catch (Exception exception)
        {
            SystemRegistryOutput.WriteError(exception.Message);
            return -1;
        }
    }
}