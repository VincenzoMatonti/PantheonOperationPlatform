using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Commands;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationSystemInstanceRequestCommandMapper
{
    public static CreateOrchestrationSystemInstanceCommand ToCreateCommand(CreateOrchestrationSystemInstanceRequest request)
    {
        return new CreateOrchestrationSystemInstanceCommand { OrchestrationId = request.OrchestrationId, SystemInstanceId = request.SystemInstanceId };
    }

    public static DeleteOrchestrationSystemInstanceCommand ToDeleteCommand(Guid id)
    {
        return new DeleteOrchestrationSystemInstanceCommand { Id = id };
    }

    public static RestoreOrchestrationSystemInstanceCommand ToRestoreCommand(Guid id)
    {
        return new RestoreOrchestrationSystemInstanceCommand { Id = id };
    }
}
