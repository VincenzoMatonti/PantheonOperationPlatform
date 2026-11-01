namespace Athena.Application.OrchestrationManagement.Queries;

public class GetOrchestrationSystemInstanceByIdQuery
{
    public Guid Id { get; set; }
}

public class GetOrchestrationSystemInstancesQuery
{
    public bool IncludeDeleted { get; set; }
}

public sealed record GetOrchestrationSystemInstancesByOrchestrationIdQuery(Guid OrchestrationId);