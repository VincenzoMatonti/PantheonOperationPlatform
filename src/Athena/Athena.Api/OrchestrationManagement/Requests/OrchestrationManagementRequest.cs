namespace Athena.Api.OrchestrationManagement.Requests;

public class SetOrchestrationDesiredStateRequest
{
    public string DesiredState { get; init; } = string.Empty;
}

public class GetOrchestrationLifecyclesByDesiredStateRequest
{
    public string State { get; init; } = string.Empty;
}

public class GetOrchestrationLifecyclesByOperationalStatusRequest
{
    public string Status { get; init; } = string.Empty;
}

public class CreateOrchestrationLifecycleOperationRequest
{
    public Guid Id { get; init; }

    public string Type { get; init; } = string.Empty;
}

public class FailOrchestrationLifecycleOperationRequest
{
    public string ErrorMessage { get; init; } = string.Empty;
}

public class GetOrchestrationLifecycleOperationsByOrchestrationIdRequest
{
    public Guid OrchestrationId { get; init; }
}

public class GetOrchestrationLifecycleOperationsByTypeRequest
{
    public string Type { get; init; } = string.Empty;
}

public class GetOrchestrationLifecycleOperationsByStatusRequest
{
    public string Status { get; init; } = string.Empty;
}

public class GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusRequest
{
    public Guid OrchestrationId { get; init; }

    public string Status { get; init; } = string.Empty;
}

public class CreateOrchestrationSystemRequest
{
    public Guid OrchestrationId { get; init; }

    public Guid SystemId { get; init; }
}

public class GetOrchestrationSystemsRequest
{
    public bool IncludeDeleted { get; init; }
}