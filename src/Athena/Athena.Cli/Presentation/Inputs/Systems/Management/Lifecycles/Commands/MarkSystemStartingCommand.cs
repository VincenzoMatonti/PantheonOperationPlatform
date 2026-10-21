using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Services.Systems;

namespace Athena.Cli.Presentation.Inputs.Systems.Management.Lifecycles.Commands;

public class MarkSystemStartingCommand(SystemManagementService service) : SystemLifecycleActionCommand(service)
{
    protected override Task<SystemLifecycleResponse> ExecuteActionAsync(Guid systemId, CancellationToken cancellationToken) =>
        Service.MarkStartingAsync(systemId, cancellationToken);
}