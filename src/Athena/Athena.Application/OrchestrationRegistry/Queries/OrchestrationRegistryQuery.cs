using Athena.Application.OrchestrationRegistry.Enums;

namespace Athena.Application.OrchestrationRegistry.Queries;

public class GetOrchestrationByIdQuery
{
    public Guid OrchestrationId { get; init; }
}

public class GetOrchestrationByNameQuery
{
    public string Name { get; init; } = string.Empty;
}


public class GetOrchestrationsByRegistrationStatusQuery
{
    public OrchestrationRegistrationStatusFilter Status { get; init; }
}

public class GetOrchestrationsByAvailabilityStatusQuery
{
    public OrchestrationAvailabilityStatusFilter Status { get; init; }
}

