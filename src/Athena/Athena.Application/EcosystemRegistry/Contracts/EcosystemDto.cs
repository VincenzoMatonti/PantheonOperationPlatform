namespace Athena.Application.EcosystemRegistry.Contracts;

public sealed class EcosystemDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public required string Name { get; set; }
    public required string RegistrationStatus { get; set; }
    public required string AvailabilityStatus { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}
