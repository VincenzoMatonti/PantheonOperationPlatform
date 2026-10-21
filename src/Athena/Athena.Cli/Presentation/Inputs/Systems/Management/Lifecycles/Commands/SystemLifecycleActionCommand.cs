using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Commands;

public abstract class SystemLifecycleActionCommand(SystemManagementService service) : AsyncCommand<SystemLifecycleActionCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        public override ValidationResult Validate() => SystemId == Guid.Empty ? ValidationResult.Error("System ID cannot be empty.") : ValidationResult.Success();
    }

    protected abstract Task<SystemLifecycleResponse> ExecuteActionAsync(Guid systemId, CancellationToken cancellationToken);

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var lifecycle = await ExecuteActionAsync(settings.SystemId, cancellationToken);
        SystemManagementOutput.RenderLifecycle(lifecycle);
        return 0;
    }

    protected SystemManagementService Service { get; } = service;
}
