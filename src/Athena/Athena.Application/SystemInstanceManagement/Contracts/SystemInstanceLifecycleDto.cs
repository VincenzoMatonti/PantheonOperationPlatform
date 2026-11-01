namespace Athena.Application.SystemInstanceManagement.Contracts;

public sealed record SystemInstanceLifecycleDto(
    Guid SystemInstanceId,
    string DesiredState,
    string OperationalStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    DateTime? ObservedAtUtc,
    bool IsDeleted);