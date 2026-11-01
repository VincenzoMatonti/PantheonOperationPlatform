namespace Athena.Api.SystemManagement.Responses;

public sealed record SystemInstanceResponse(
    Guid Id,
    Guid SystemId,
    Guid WorkloadId,
    string Name,
    string RegistrationStatus,
    string DesiredState,
    string OperationalStatus,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    DateTime? ObservedAtUtc);

public sealed record SystemInstanceOperationResponse(
    Guid Id,
    Guid SystemInstanceId,
    string Type,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    string? ErrorMessage,
    DateTime? AgentAcceptedAtUtc,
    DateTime? AgentCompletedAtUtc,
    DateTime? LeaseExpiresAtUtc,
    int AttemptCount,
    string? ConvergenceStatus,
    int MaxAttempts);

public sealed record SystemInstanceExecutionStatusResponse(
    Guid SystemInstanceId,
    string State,
    bool IsRunning,
    bool IsHealthy,
    DateTime ObservedAtUtc,
    string? Details);

public sealed record SystemInstanceExecutionHealthResponse(
    Guid SystemInstanceId,
    bool IsHealthy,
    string Status,
    DateTime CheckedAtUtc,
    string? Details);

public sealed record SystemInstanceExecutionLogResponse(DateTime TimestampUtc, string Stream, string Message);
