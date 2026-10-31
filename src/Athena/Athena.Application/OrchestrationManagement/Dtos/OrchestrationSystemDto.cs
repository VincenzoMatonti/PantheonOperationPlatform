namespace Athena.Application.OrchestrationManagement.DTOs;

public class OrchestrationSystemDto
{
    public Guid Id { get; set; }
    public Guid OrchestrationId { get; set; }
    public Guid SystemId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}