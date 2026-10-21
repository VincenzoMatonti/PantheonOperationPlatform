using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;

public class GetSystemsByAvailabilityStatusCommand(SystemRegistryService service) : AsyncCommand<GetSystemsByAvailabilityStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<status>")]
        public string Status { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            var systems = await service.GetByAvailabilityStatusAsync(settings.Status, cancellationToken);
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