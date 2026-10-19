using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Api.OrchestrationManagement.Mappings;

public static class OrchestrationLifecycleRequestQueryMapper
{
    public static GetOrchestrationLifecycleByIdQuery ToGetByIdQuery(Guid orchestrationId)
    {
        return new GetOrchestrationLifecycleByIdQuery { OrchestrationId = orchestrationId };
    }

    public static GetOrchestrationLifecyclesByDesiredStateQuery ToGetByDesiredStateQuery(GetOrchestrationLifecyclesByDesiredStateRequest request)
    {
        return new GetOrchestrationLifecyclesByDesiredStateQuery
        {
            State = Enum.TryParse<OrchestrationDesiredState>(request.State, true, out var state) ? state : default
        };
    }

    public static GetOrchestrationLifecyclesByOperationalStatusQuery ToGetByOperationalStatusQuery(GetOrchestrationLifecyclesByOperationalStatusRequest request)
    {
        return new GetOrchestrationLifecyclesByOperationalStatusQuery
        {
            Status = Enum.TryParse<OrchestrationOperationalStatus>(request.Status, true, out var status) ? status : default
        };
    }
}
