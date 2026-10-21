using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Registry.Queries;

public class GetSystemsByCapabilityCommand(SystemRegistryService service) : AsyncCommand<GetSystemsByCapabilityCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--name <NAME>")]
        public string Name { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(settings.Name))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Specify a capability with --name.");
            return -1;
        }

        try
        {
            var systems = await service.GetByCapabilityAsync(settings.Name, cancellationToken);
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