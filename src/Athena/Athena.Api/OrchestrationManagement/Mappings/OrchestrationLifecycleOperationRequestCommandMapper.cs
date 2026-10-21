using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationLifecycleOperationRequestCommandMapper
{
    public static CreateOrchestrationLifecycleOperationCommand ToCreateOperationCommand(Guid orchestrationId, CreateOrchestrationLifecycleOperationRequest request)
    {
        return new CreateOrchestrationLifecycleOperationCommand
        {
            Id = request.Id,
            OrchestrationId = orchestrationId,
            Type = Enum.TryParse<OrchestrationLifecycleOperationType>(request.Type, true, out var type) ? type : default
        };
    }

    public static StartOrchestrationLifecycleOperationCommand ToStartOperationCommand(Guid operationId)
    {
        return new StartOrchestrationLifecycleOperationCommand { Id = operationId };
    }

    public static CompleteOrchestrationLifecycleOperationCommand ToCompleteOperationCommand(Guid operationId)
    {
        return new CompleteOrchestrationLifecycleOperationCommand { Id = operationId };
    }

    public static FailOrchestrationLifecycleOperationCommand ToFailOperationCommand(Guid operationId, FailOrchestrationLifecycleOperationRequest request)
    {
        return new FailOrchestrationLifecycleOperationCommand { Id = operationId, ErrorMessage = request.ErrorMessage };
    }

    public static CancelOrchestrationLifecycleOperationCommand ToCancelOperationCommand(Guid operationId)
    {
        return new CancelOrchestrationLifecycleOperationCommand { Id = operationId };
    }

    public static DeleteOrchestrationLifecycleOperationCommand ToDeleteOperationCommand(Guid operationId)
    {
        return new DeleteOrchestrationLifecycleOperationCommand { Id = operationId };
    }
}
