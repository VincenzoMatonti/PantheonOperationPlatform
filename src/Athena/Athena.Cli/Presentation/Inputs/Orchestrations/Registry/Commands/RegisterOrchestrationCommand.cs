using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Registry.Commands;

public class RegisterOrchestrationCommand(OrchestrationRegistryService service) : AsyncCommand<RegisterOrchestrationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ID>")]
        public Guid Id { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (settings.Id == Guid.Empty)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Specify a valid orchestration ID.");
            return -1;
        }

        try
        {
            var orchestration = await service.RegisterAsync(settings.Id, cancellationToken);
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
