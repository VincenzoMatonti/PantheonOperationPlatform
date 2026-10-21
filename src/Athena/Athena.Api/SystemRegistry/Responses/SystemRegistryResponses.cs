
namespace Athena.Api.SystemRegistry.Responses;

public class SystemResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string RegistrationStatus { get; init; }
    public required string AvailabilityStatus { get; init; }
    public IReadOnlyList<SystemEndpointResponse> Endpoints { get; init; } = [];
    public IReadOnlyList<SystemCapabilityResponse> Capabilities { get; init; } = [];
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public bool IsDeleted { get; init; }
}

public class SystemEndpointResponse
{
    public required string Name { get; init; }
    public required string BaseAddress { get; init; }
}

public class SystemCapabilityResponse
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
