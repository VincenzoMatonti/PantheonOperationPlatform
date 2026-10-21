using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Registry.Queries;

public class GetOrchestrationsByAvailabilityStatusCommand(OrchestrationRegistryService service) : AsyncCommand<GetOrchestrationsByAvailabilityStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<STATUS>")]
        public string Status { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(settings.Status))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Specify an availability status.");
            return -1;
        }

        try
        {
            var orchestrations = await service.GetByAvailabilityStatusAsync(settings.Status, cancellationToken);
            OrchestrationRegistryOutput.WriteOrchestrations(orchestrations);
            return 0;
        }
        catch (Exception exception)
        {
            OrchestrationRegistryOutput.WriteError(exception.Message);
            return -1;
        }
    }
}
