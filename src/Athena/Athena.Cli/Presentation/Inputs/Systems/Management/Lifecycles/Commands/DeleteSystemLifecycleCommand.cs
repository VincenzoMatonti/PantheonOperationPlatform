using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Commands;

public class DeleteSystemLifecycleCommand(SystemManagementService service) : AsyncCommand<DeleteSystemLifecycleCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<SYSTEM_ID>")]
        public Guid SystemId { get; init; }

        public override ValidationResult Validate() => SystemId == Guid.Empty ? ValidationResult.Error("System ID cannot be empty.") : ValidationResult.Success();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        await service.DeleteLifecycleAsync(settings.SystemId, cancellationToken);
        SystemManagementOutput.RenderSuccess($"Lifecycle for system '{settings.SystemId}' deleted.");
        return 0;
    }
}
