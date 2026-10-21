using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Commands;

public  class SetSystemDesiredStateCommand(SystemManagementService service) : AsyncCommand<SetSystemDesiredStateCommand.Settings>
{
    public  class Settings : CommandSettings
    {
        [CommandArgument(0, "<SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        [CommandArgument(1, "<DESIRED_STATE>")]
        public string DesiredState { get; init; } = string.Empty;

        public override ValidationResult Validate()
        {
            if (SystemId == Guid.Empty) return ValidationResult.Error("System ID cannot be empty.");
            return string.IsNullOrWhiteSpace(DesiredState) ? ValidationResult.Error("Desired state is required.") : ValidationResult.Success();
        }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var lifecycle = await service.SetDesiredStateAsync(settings.SystemId, new SetSystemDesiredStateRequest(settings.DesiredState), cancellationToken);
        SystemManagementOutput.RenderLifecycle(lifecycle);
        return 0;
    }
}
