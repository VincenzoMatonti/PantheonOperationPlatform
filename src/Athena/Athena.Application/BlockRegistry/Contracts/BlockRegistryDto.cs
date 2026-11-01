namespace Athena.Application.BlockRegistry.Contracts;

public class BlockDto
{
    public Guid Id { get; set; }
    public Guid EcosystemId { get; set; }
    public required string Name { get; set; }
    public required string RegistrationStatus { get; set; }
    public required string AvailabilityStatus { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}