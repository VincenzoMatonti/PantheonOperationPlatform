using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Spectre.Console.Cli;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Queries;

public class ListDeletedSystemLifecycleOperationsCommand(SystemManagementService service) : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        var operations = await service.GetDeletedOperationsAsync(cancellationToken);
        SystemManagementOutput.RenderOperations(operations);
        return 0;
    }
}
