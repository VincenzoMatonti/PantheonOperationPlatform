using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Queries;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationSystemRequestQueryMapper
{
    public static GetOrchestrationSystemByIdQuery ToGetByIdQuery(Guid id)
    {
        return new GetOrchestrationSystemByIdQuery { Id = id };
    }

    public static GetOrchestrationSystemsQuery ToGetAllQuery(GetOrchestrationSystemsRequest request)
    {
        return new GetOrchestrationSystemsQuery { IncludeDeleted = request.IncludeDeleted };
    }
}
