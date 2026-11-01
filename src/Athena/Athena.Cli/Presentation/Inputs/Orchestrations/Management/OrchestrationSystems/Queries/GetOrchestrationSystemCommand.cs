using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.OrchestrationSystemInstances.Queries;

public class GetOrchestrationSystemInstanceCommand(OrchestrationSystemInstanceManagementService service) : AsyncCommand<GetOrchestrationSystemInstanceCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ASSOCIATION_ID>")]
        public Guid Id { get; init; }

        public override ValidationResult Validate() =>
            Id == Guid.Empty ? ValidationResult.Error("Association ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var association = await service.GetByIdAsync(settings.Id, cancellationToken);
        OrchestrationSystemInstanceManagementOutput.Write(association);
        return 0;
    }
}