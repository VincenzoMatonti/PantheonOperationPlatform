using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Queries;

public class GetSystemLifecyclesByDeletionStatusCommand(SystemManagementService service) : AsyncCommand<GetSystemLifecyclesByDeletionStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--deleted")]
        public bool IsDeleted { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var lifecycles = await service.GetLifecyclesByDeletionStatusAsync(settings.IsDeleted, cancellationToken);
        SystemManagementOutput.RenderLifecycles(lifecycles);
        return 0;
    }
}
