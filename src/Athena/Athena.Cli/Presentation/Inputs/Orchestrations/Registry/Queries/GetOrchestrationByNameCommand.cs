using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Registry.Queries;

public class GetOrchestrationByNameCommand(OrchestrationRegistryService service) : AsyncCommand<GetOrchestrationByNameCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<NAME>")]
        public string Name { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(settings.Name))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Specify an orchestration name.");
            return -1;
        }

        try
        {
            var orchestration = await service.GetByNameAsync(settings.Name, cancellationToken);
            OrchestrationRegistryOutput.WriteOrchestration(orchestration);
            return 0;
        }
        catch (Exception exception)
        {
            OrchestrationRegistryOutput.WriteError(exception.Message);
            return -1;
        }
    }
}
