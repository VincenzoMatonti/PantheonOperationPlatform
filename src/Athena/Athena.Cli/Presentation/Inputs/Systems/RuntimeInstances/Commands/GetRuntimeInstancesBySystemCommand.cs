using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.RuntimeInstances.Commands;

public class GetRuntimeInstancesBySystemCommand(SystemRuntimeInstancesService service) : AsyncCommand<GetRuntimeInstancesBySystemCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        public override ValidationResult Validate() =>
            SystemId == Guid.Empty ? ValidationResult.Error("System ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var instances = await service.GetBySystemIdAsync(settings.SystemId, cancellationToken);
        SystemRuntimeInstanceOutput.RenderBySystem(settings.SystemId, instances);
        return 0;
    }
}
