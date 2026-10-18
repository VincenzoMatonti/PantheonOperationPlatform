using Athena.Application.SystemManagement.Enums;

namespace Athena.Application.SystemManagement.Queries;

public class GetSystemLifecycleBySystemIdQuery
{
    public Guid SystemId { get; set; }
}

public class GetAllSystemLifecyclesQuery
{
}

public class GetSystemLifecyclesByDesiredStateQuery
{
    public SystemDesiredStateFilter DesiredState { get; set; }
}

public class GetSystemLifecyclesByOperationalStatusQuery
{
    public SystemOperationalStatusFilter OperationalStatus { get; set; }
}

public class GetSystemLifecyclesByDeletionStatusQuery
{
    public bool IsDeleted { get; set; }
}