namespace Athena.Application.OrchestrationManagement.Queries;

public class GetOrchestrationSystemByIdQuery
{
    public Guid Id { get; set; }
}

public class GetOrchestrationSystemsQuery
{
    public bool IncludeDeleted { get; set; }
}