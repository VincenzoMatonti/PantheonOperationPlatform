using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Commands;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationSystemRequestCommandMapper
{
    public static CreateOrchestrationSystemCommand ToCreateCommand(CreateOrchestrationSystemRequest request)
    {
        return new CreateOrchestrationSystemCommand { OrchestrationId = request.OrchestrationId, SystemId = request.SystemId };
    }

    public static DeleteOrchestrationSystemCommand ToDeleteCommand(Guid id)
    {
        return new DeleteOrchestrationSystemCommand { Id = id };
    }

    public static RestoreOrchestrationSystemCommand ToRestoreCommand(Guid id)
    {
        return new RestoreOrchestrationSystemCommand { Id = id };
    }
}
