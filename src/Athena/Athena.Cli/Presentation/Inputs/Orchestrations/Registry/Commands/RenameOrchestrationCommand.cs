using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Registry.Commands;

public class RenameOrchestrationCommand(OrchestrationRegistryService service) : AsyncCommand<RenameOrchestrationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ID>")]
        public Guid Id { get; set; }

        [CommandOption("--name <NAME>")]
        public string Name { get; set; } = string.Empty;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            if (settings.Id == Guid.Empty) throw new ArgumentException("Orchestration ID cannot be empty.");
            if (string.IsNullOrWhiteSpace(settings.Name)) throw new ArgumentException("Orchestration name is required.");
            var orchestration = await service.RenameAsync(settings.Id, new RenameOrchestrationRequest(settings.Name), cancellationToken);
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
