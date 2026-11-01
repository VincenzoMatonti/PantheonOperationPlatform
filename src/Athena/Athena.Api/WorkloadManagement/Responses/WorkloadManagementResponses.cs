namespace Athena.Api.WorkloadManagement.Responses;

public sealed record WorkloadLifecycleResponse(
    Guid WorkloadId,
    string DesiredState,
    string OperationalStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    bool IsDeleted);

public sealed record WorkloadOperationResponse(
    Guid Id,
    Guid WorkloadId,
    string Type,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    string? ErrorMessage,
    bool IsDeleted);
