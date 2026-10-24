using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.RuntimeInstances.Commands;

public class UnregisterRuntimeInstanceCommand(SystemRuntimeInstancesService service) : AsyncCommand<UnregisterRuntimeInstanceCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<RUNTIME_INSTANCE_ID>")]
        public Guid RuntimeInstanceId { get; init; }

        public override ValidationResult Validate() =>
            RuntimeInstanceId == Guid.Empty ? ValidationResult.Error("Runtime instance ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var removed = await service.UnregisterAsync(settings.RuntimeInstanceId, cancellationToken);
        if (!removed)
        {
            SystemRuntimeInstanceOutput.RenderNotFound(settings.RuntimeInstanceId.ToString());
            return 1;
        }

        SystemRuntimeInstanceOutput.RenderUnregistered(settings.RuntimeInstanceId);
        return 0;
    }
}
