
namespace Athena.Api.SystemRegistry.Requests;

public class RegisterSystemRequest
{
    public required string Name { get; init; }
}

public class RenameSystemRequest
{
    public required string NewName { get; init; }
}

public class AddSystemEndpointRequest
{
    public required string Name { get; init; }
    public required string BaseAddress { get; init; }
}

public class RemoveSystemEndpointRequest
{
    public required string EndpointName { get; init; }
}

public class AddSystemCapabilityRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}

public class RemoveSystemCapabilityRequest
{
    public required string CapabilityName { get; init; }
}
