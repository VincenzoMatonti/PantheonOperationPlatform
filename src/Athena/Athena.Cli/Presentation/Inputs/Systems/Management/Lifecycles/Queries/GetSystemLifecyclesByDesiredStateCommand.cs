using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Queries;

public class GetSystemLifecyclesByDesiredStateCommand(SystemManagementService service) : AsyncCommand<GetSystemLifecyclesByDesiredStateCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<DESIRED_STATE>")]
        public string DesiredState { get; init; } = string.Empty;

        public override ValidationResult Validate() =>
          string.IsNullOrWhiteSpace(DesiredState) ? ValidationResult.Error("Desired state is required.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var lifecycles = await service.GetLifecyclesByDesiredStateAsync(settings.DesiredState, cancellationToken);
        SystemManagementOutput.RenderLifecycles(lifecycles);
        return 0;
    }
}
