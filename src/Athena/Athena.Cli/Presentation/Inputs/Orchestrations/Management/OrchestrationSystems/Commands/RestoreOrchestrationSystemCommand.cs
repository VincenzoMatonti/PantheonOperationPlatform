using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.OrchestrationSystems.Commands;

public class RestoreOrchestrationSystemCommand(OrchestrationSystemManagementService service) : AsyncCommand<RestoreOrchestrationSystemCommand.Settings>
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
        var association = await service.RestoreAsync(settings.Id, cancellationToken);
        OrchestrationSystemManagementOutput.Write(association);
        return 0;
    }
}