using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Presentation.Outputs.Orchestrations;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Orchestrations.Management.OrchestrationSystems.Commands;

public class CreateOrchestrationSystemCommand(OrchestrationSystemManagementService service) : AsyncCommand<CreateOrchestrationSystemCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<ORCHESTRATION_ID>")]
        public Guid OrchestrationId { get; init; }

        [CommandArgument(1, "<SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        public override ValidationResult Validate()
        {
            if (OrchestrationId == Guid.Empty) return ValidationResult.Error("Orchestration ID cannot be empty.");
            if (SystemId == Guid.Empty) return ValidationResult.Error("System ID cannot be empty.");
            return ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var request = new CreateOrchestrationSystemRequest(settings.OrchestrationId, settings.SystemId);
        var association = await service.CreateAsync(request, cancellationToken);
        OrchestrationSystemManagementOutput.Write(association);
        return 0;
    }
}