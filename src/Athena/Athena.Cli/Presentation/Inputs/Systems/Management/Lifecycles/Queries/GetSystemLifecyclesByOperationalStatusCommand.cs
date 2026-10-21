using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Queries;

public class GetSystemLifecyclesByOperationalStatusCommand(SystemManagementService service) : AsyncCommand<GetSystemLifecyclesByOperationalStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<OPERATIONAL_STATUS>")]
        public string OperationalStatus { get; init; } = string.Empty;

        public override ValidationResult Validate() =>
            string.IsNullOrWhiteSpace(OperationalStatus) ? ValidationResult.Error("Operational status is required.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var lifecycles = await service.GetLifecyclesByOperationalStatusAsync(settings.OperationalStatus, cancellationToken);
        SystemManagementOutput.RenderLifecycles(lifecycles);
        return 0;
    }
}
