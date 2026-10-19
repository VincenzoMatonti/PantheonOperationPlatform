namespace Athena.Api.SystemManagement.Responses;

public class SystemLifecycleQueryResponse
{
    public Guid SystemId { get; init; }

    public string DesiredState { get; init; } = string.Empty;

    public string OperationalStatus { get; init; } = string.Empty;

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public bool IsDeleted { get; init; }
}

public class SystemLifecycleOperationQueryResponse
{
    public Guid Id { get; init; }

    public Guid SystemId { get; init; }

    public string Type { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public DateTime CreatedAtUtc { get; init; }

    public DateTime? UpdatedAtUtc { get; init; }

    public DateTime? StartedAtUtc { get; init; }

    public DateTime? CompletedAtUtc { get; init; }

    public string? ErrorMessage { get; init; }

    public bool IsDeleted { get; init; }
}