using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Application.OrchestrationManagement.Queries
{
    public sealed record GetAllOrchestrationLifecyclesQuery;
    public sealed record GetDeletedOrchestrationLifecyclesQuery;

    public class GetOrchestrationLifecycleByIdQuery
    {
        public Guid OrchestrationId { get; init; }
    }

    public class GetOrchestrationLifecyclesByDesiredStateQuery
    {
        public OrchestrationDesiredState State { get; init; }
    }

    public class GetOrchestrationLifecyclesByOperationalStatusQuery
    {
        public OrchestrationOperationalStatus Status { get; init; }
    }
}
