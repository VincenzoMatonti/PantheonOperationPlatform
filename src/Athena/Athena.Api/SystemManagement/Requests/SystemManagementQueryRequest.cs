using Athena.Application.SystemManagement.Enums;

namespace Athena.Api.SystemManagement.Requests;

public class GetSystemLifecyclesByDesiredStateRequest
{
    public SystemDesiredStateFilter DesiredState { get; init; }
}

public class GetSystemLifecyclesByOperationalStatusRequest
{
    public SystemOperationalStatusFilter OperationalStatus { get; init; }
}

public class GetSystemLifecyclesByDeletionStatusRequest
{
    public bool IsDeleted { get; init; }
}

public class GetSystemLifecycleOperationsByStatusRequest
{
    public SystemLifecycleOperationStatusFilter Status { get; init; }
}

public class GetSystemLifecycleOperationsBySystemIdAndStatusRequest
{
    public SystemLifecycleOperationStatusFilter Status { get; init; }
}

public class GetSystemLifecycleOperationsByDeletionStatusRequest
{
    public bool IsDeleted { get; init; }
}