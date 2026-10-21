namespace Athena.Application.SystemManagement.Dtos;

public class SystemLifecycleDto
{
    public Guid SystemId { get; set; }
    public string DesiredState { get; set; } = string.Empty;
    public string OperationalStatus { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}
