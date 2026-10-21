using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Application.OrchestrationManagement.DTOs;

public class OrchestrationLifecycleOperationDto
{
    public Guid Id { get; init; }

    public Guid OrchestrationId { get; init; }

    public OrchestrationLifecycleOperationType Type { get; init; }

    public OrchestrationLifecycleOperationStatus Status { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public DateTime? StartedAtUtc { get; init; }

    public DateTime? CompletedAtUtc { get; init; }

    public string? ErrorMessage { get; init; }

    public bool IsDeleted { get; init; }
}
