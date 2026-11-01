namespace Athena.Cli.Application.Contracts.Systems;

public sealed record SystemInstanceProcessResponse(
    Guid Id,
    Guid SystemId,
    Guid WorkloadId,
    string Name,
    string RegistrationStatus,
    string DesiredState,
    string OperationalStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    DateTime? ObservedAtUtc = null);

public sealed record SystemInstanceStatusResponse(
    Guid SystemInstanceId,
    string State,
    bool IsRunning,
    bool IsHealthy,
    DateTime ObservedAtUtc,
    string? Details = null);

public sealed record SystemInstanceHealthResponse(
    Guid SystemInstanceId,
    bool IsHealthy,
    string Status,
    DateTime CheckedAtUtc,
    string? Details = null);

public sealed record SystemInstanceLogResponse(DateTime TimestampUtc, string Stream, string Message);

public sealed record SystemInstanceOperationResponse(
    Guid Id,
    Guid SystemInstanceId,
    string Type,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    string? ErrorMessage,
    DateTime? AgentAcceptedAtUtc = null,
    DateTime? AgentCompletedAtUtc = null,
    DateTime? LeaseExpiresAtUtc = null,
    int AttemptCount = 0,
    string? ConvergenceStatus = null,
    int MaxAttempts = 8);
