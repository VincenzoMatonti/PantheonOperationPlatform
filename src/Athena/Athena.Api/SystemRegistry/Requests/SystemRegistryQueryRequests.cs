
namespace Athena.Api.SystemRegistry.Requests;

public class GetSystemByNameRequest
{
    public required string Name { get; init; }
}

public class GetSystemsByRegistrationStatusRequest
{
    public required string Status { get; init; }
}

public class GetSystemsByAvailabilityStatusRequest
{
    public required string Status { get; init; }
}

public class GetSystemsByCapabilityRequest
{
    public required string CapabilityName { get; init; }
}

public class GetSystemsByDeletionStatusRequest
{
    public bool IsDeleted { get; init; }
}
