using Athena.Application.SystemRegistry.Enums;

namespace Athena.Application.SystemRegistry.Queries;

public class GetSystemByIdQuery
{
    public Guid SystemId { get; set; }
}

public class GetSystemByNameQuery
{
    public required string Name { get; set; }
}

public class GetAllSystemsQuery
{
}

public class GetSystemsByRegistrationStatusQuery
{
    public SystemRegistrationStatusFilter Status { get; set; }
}

public class GetSystemsByAvailabilityStatusQuery
{
    public SystemAvailabilityStatusFilter Status { get; set; }
}

public class GetSystemsByCapabilityQuery
{
    public required string CapabilityName { get; set; }
}

public class GetSystemEndpointsQuery
{
    public Guid SystemId { get; set; }
}

public class GetSystemCapabilitiesQuery
{
    public Guid SystemId { get; set; }
}

public class GetSystemsByDeletionStatusQuery
{
    public bool IsDeleted { get; set; }
}

public class GetDeletedSystemsQuery
{
}