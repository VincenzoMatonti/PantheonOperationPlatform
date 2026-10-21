namespace Athena.Application.SystemRegistry.Dtos;

public class SystemDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string RegistrationStatus { get; set; }
    public required string AvailabilityStatus { get; set; }
    public IReadOnlyList<SystemEndpointDto> Endpoints { get; set; } = [];
    public IReadOnlyList<SystemCapabilityDto> Capabilities { get; set; } = [];
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}

public class SystemEndpointDto
{
    public required string Name { get; set; }
    public required string BaseAddress { get; set; }
}

public class SystemCapabilityDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}