
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.RuntimeInstances.Commands;

public class GetRuntimeInstanceCommand(
    SystemRuntimeInstancesService service)
    : AsyncCommand<GetRuntimeInstanceCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<RUNTIME_INSTANCE_ID>")]
        public Guid RuntimeInstanceId { get; init; }

        public override ValidationResult Validate() =>
            RuntimeInstanceId == Guid.Empty ? ValidationResult.Error("Runtime instance ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var instance = await service.GetByIdAsync(settings.RuntimeInstanceId, cancellationToken);
        if (instance is null)
        {
            SystemRuntimeInstanceOutput.RenderNotFound(settings.RuntimeInstanceId.ToString());
            return 1;
        }

        SystemRuntimeInstanceOutput.RenderDetails(instance);

        return 0;
    }
}
