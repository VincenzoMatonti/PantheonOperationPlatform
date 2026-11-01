namespace Athena.Api.BlockManagement.Responses;

public sealed record BlockLifecycleResponse(
    Guid BlockId,
    string DesiredState,
    string OperationalStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    bool IsDeleted);

public sealed record BlockOperationResponse(
    Guid Id,
    Guid BlockId,
    string Type,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    string? ErrorMessage,
    bool IsDeleted);
