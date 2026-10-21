namespace Athena.Application.SystemManagement.Dtos;

public class SystemLifecycleOperationDto
{
    public Guid Id { get; set; }
    public Guid SystemId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsDeleted { get; set; }
}
