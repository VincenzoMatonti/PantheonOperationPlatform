namespace Athena.Application.GroupRegistry.Contracts;

public class GroupDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string RegistrationStatus { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}