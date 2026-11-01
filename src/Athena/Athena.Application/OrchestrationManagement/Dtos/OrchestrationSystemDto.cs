namespace Athena.Application.OrchestrationManagement.DTOs;

public class OrchestrationSystemInstanceDto
{
    public Guid Id { get; set; }
    public Guid OrchestrationId { get; set; }
    public Guid SystemInstanceId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}