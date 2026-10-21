using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Queries;

public class GetSystemLifecycleOperationsByDeletionStatusCommand(SystemManagementService service) : AsyncCommand<GetSystemLifecycleOperationsByDeletionStatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--deleted")]
        public bool IsDeleted { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var operations = await service.GetOperationsByDeletionStatusAsync(settings.IsDeleted, cancellationToken);
        SystemManagementOutput.RenderOperations(operations);
        return 0;
    }
}
