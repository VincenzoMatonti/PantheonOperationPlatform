using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Operations.Commands;

public sealed class CancelSystemLifecycleOperationCommand(SystemManagementService service) : SystemLifecycleOperationActionCommand(service)
{
    protected override Task<SystemLifecycleOperationResponse> ExecuteActionAsync(Guid operationId, CancellationToken cancellationToken) =>
        Service.CancelOperationAsync(operationId, cancellationToken);
}