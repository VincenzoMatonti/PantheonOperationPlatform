using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Registry.Commands;

public class DeleteOrchestrationCommand(OrchestrationRegistryService service) : AsyncCommand<DeleteOrchestrationCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ID>")]
        public Guid Id { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            if (settings.Id == Guid.Empty) throw new ArgumentException("Orchestration ID cannot be empty.");
            var orchestration = await service.DeleteAsync(settings.Id, cancellationToken);
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
