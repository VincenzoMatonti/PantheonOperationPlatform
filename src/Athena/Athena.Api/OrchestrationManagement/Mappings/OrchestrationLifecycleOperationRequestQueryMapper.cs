using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationLifecycleOperationRequestQueryMapper
{
    public static GetOrchestrationLifecycleOperationByIdQuery ToGetByIdQuery(Guid operationId)
    {
        return new GetOrchestrationLifecycleOperationByIdQuery { Id = operationId };
    }

    public static GetOrchestrationLifecycleOperationsByOrchestrationIdQuery ToGetByOrchestrationIdQuery(GetOrchestrationLifecycleOperationsByOrchestrationIdRequest request)
    {
        return new GetOrchestrationLifecycleOperationsByOrchestrationIdQuery { OrchestrationId = request.OrchestrationId };
    }

    public static GetOrchestrationLifecycleOperationsByTypeQuery ToGetByTypeQuery(GetOrchestrationLifecycleOperationsByTypeRequest request)
    {
        return new GetOrchestrationLifecycleOperationsByTypeQuery
        {
            Type = Enum.TryParse<OrchestrationLifecycleOperationType>(request.Type, true, out var type) ? type : default
        };
    }

    public static GetOrchestrationLifecycleOperationsByStatusQuery ToGetByStatusQuery(GetOrchestrationLifecycleOperationsByStatusRequest request)
    {
        return new GetOrchestrationLifecycleOperationsByStatusQuery
        {
            Status = Enum.TryParse<OrchestrationLifecycleOperationStatus>(request.Status, true, out var status) ? status : default
        };
    }

    public static GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusQuery ToGetByOrchestrationIdAndStatusQuery(
        GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusRequest request)
    {
        return new GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusQuery
        {
            OrchestrationId = request.OrchestrationId,
            Status = Enum.TryParse<OrchestrationLifecycleOperationStatus>(request.Status, true, out var status) ? status : default
        };
    }
}
