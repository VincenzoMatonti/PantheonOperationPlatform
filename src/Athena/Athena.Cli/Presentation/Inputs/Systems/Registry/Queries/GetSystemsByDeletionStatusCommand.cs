using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;

public class GetSystemsByDeletionStatusCommand(SystemRegistryService service) : AsyncCommand<GetSystemsByDeletionStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--deleted")]
        public bool Deleted { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            var systems = await service.GetByDeletionStatusAsync(settings.Deleted, cancellationToken);
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