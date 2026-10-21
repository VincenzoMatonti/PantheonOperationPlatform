using Athena.Api.OrchestrationRegistry.Requests;
using Athena.Application.OrchestrationRegistry.Enums;
using Athena.Application.OrchestrationRegistry.Queries;

namespace Athena.Api.OrchestrationRegistry.Mappings;

public static class OrchestrationRegistryQueryRequestMapper
{
    public static GetOrchestrationByIdQuery ToByIdQuery(Guid orchestrationId)
    {
        return new GetOrchestrationByIdQuery { OrchestrationId = orchestrationId };
    }

    public static GetOrchestrationByNameQuery ToByNameQuery(GetOrchestrationByNameRequest request)
    {
        return new GetOrchestrationByNameQuery { Name = request.Name };
    }

    public static GetOrchestrationsByRegistrationStatusQuery ToByRegistrationStatusQuery(OrchestrationRegistrationStatusFilter status)
    {
        return new GetOrchestrationsByRegistrationStatusQuery { Status = status };
    }

    public static GetOrchestrationsByAvailabilityStatusQuery ToByAvailabilityStatusQuery(OrchestrationAvailabilityStatusFilter status)
    {
        return new GetOrchestrationsByAvailabilityStatusQuery { Status = status };
    }
}