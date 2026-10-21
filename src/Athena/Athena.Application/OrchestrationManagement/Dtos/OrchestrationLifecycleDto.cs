using Athena.Application.OrchestrationManagement.Enums;

namespace Athena.Application.OrchestrationManagement.DTOs;

public class OrchestrationLifecycleDto
{
    public Guid OrchestrationId { get; init; }

    public OrchestrationDesiredState DesiredState { get; init; }

    public OrchestrationOperationalStatus OperationalStatus { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public bool IsDeleted { get; init; }
}
