namespace Athena.Api.OrchestrationRegistry.Responses;

public class OrchestrationResponse
{
    public Guid OrchestrationId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string RegistrationStatus { get; init; } = string.Empty;

    public string AvailabilityStatus { get; init; } = string.Empty;

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public bool IsDeleted { get; init; }
}