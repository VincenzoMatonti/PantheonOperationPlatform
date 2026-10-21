using Athena.Application.SystemManagement.Enums;

namespace Athena.Application.SystemManagement.Queries;

public class GetSystemLifecycleOperationByIdQuery
{
    public Guid OperationId { get; set; }
}

public class GetSystemLifecycleOperationsBySystemIdQuery
{
    public Guid SystemId { get; set; }
}

public class GetSystemLifecycleOperationsByStatusQuery
{
    public SystemLifecycleOperationStatusFilter Status { get; set; }
}

public class GetSystemLifecycleOperationsBySystemIdAndStatusQuery
{
    public Guid SystemId { get; set; }
    public SystemLifecycleOperationStatusFilter Status { get; set; }
}

public class GetSystemLifecycleOperationsByDeletionStatusQuery
{
    public bool IsDeleted { get; set; }
}