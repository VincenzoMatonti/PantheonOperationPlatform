using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Queries;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationSystemInstanceRequestQueryMapper
{
    public static GetOrchestrationSystemInstanceByIdQuery ToGetByIdQuery(Guid id)
    {
        return new GetOrchestrationSystemInstanceByIdQuery { Id = id };
    }

    public static GetOrchestrationSystemInstancesQuery ToGetAllQuery(GetOrchestrationSystemInstancesRequest request)
    {
        return new GetOrchestrationSystemInstancesQuery { IncludeDeleted = request.IncludeDeleted };
    }
}
