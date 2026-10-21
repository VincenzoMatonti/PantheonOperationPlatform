using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;

public class GetSystemCapabilitiesCommand(SystemRegistryService service) : AsyncCommand<GetSystemCapabilitiesCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<id>")]
        public string Id { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(settings.Id, out var id))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] The system ID must be a valid GUID.");
            return -1;
        }

        try
        {
            var capabilities = await service.GetCapabilitiesAsync(id, cancellationToken);
            SystemRegistryOutput.WriteCapabilities(capabilities);
            return 0;
        }
        catch (Exception exception)
        {
            SystemRegistryOutput.WriteError(exception.Message);
            return -1;
        }
    }
}