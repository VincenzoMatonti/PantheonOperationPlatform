using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Registry.Commands;

public class CreateOrchestrationCommand(OrchestrationRegistryService service) : AsyncCommand<CreateOrchestrationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--id <ID>")]
        public Guid Id { get; set; }

        [CommandOption("--name <NAME>")]
        public string Name { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (settings.Id == Guid.Empty)
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Specify a valid orchestration ID with --id.");
            return -1;
        }

        if (string.IsNullOrWhiteSpace(settings.Name))
        {
            AnsiConsole.MarkupLine("[red]Error:[/] Specify an orchestration name with --name.");
            return -1;
        }

        try
        {
            var orchestration = await service.CreateAsync(new CreateOrchestrationRequest(settings.Id, settings.Name), cancellationToken);
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
