using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Application.OrchestrationManagement.Queries
{
    public class GetOrchestrationLifecycleOperationByIdQuery
    {
        public Guid Id { get; init; }
    }

    public class GetAllOrchestrationLifecycleOperationsQuery
    {
    }

    public class GetOrchestrationLifecycleOperationsByOrchestrationIdQuery
    {
        public Guid OrchestrationId { get; init; }
    }

    public class GetOrchestrationLifecycleOperationsByTypeQuery
    {
        public OrchestrationLifecycleOperationType Type { get; init; }
    }

    public class GetOrchestrationLifecycleOperationsByStatusQuery
    {
        public OrchestrationLifecycleOperationStatus Status { get; init; }
    }

    public class GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusQuery
    {
        public Guid OrchestrationId { get; init; }

        public OrchestrationLifecycleOperationStatus Status { get; init; }
    }

    public class GetPendingOrchestrationLifecycleOperationsQuery
    {
    }

    public class GetRunningOrchestrationLifecycleOperationsQuery
    {
    }

    public class GetFailedOrchestrationLifecycleOperationsQuery
    {
    }

    public class GetDeletedOrchestrationLifecycleOperationsQuery
    {
    }
}
