using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;

public class GetSystemEndpointsCommand(SystemRegistryService service) : AsyncCommand<GetSystemEndpointsCommand.Settings>
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
            var endpoints = await service.GetEndpointsAsync(id, cancellationToken);
            SystemRegistryOutput.WriteEndpoints(endpoints);
            return 0;
        }
        catch (Exception exception)
        {
            SystemRegistryOutput.WriteError(exception.Message);
            return -1;
        }
    }
}