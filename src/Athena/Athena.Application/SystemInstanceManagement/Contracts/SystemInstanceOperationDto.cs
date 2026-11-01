namespace Athena.Application.SystemInstanceManagement.Contracts;

public sealed record SystemInstanceOperationDto(
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

public sealed record ResolveSystemInstanceOperationRequest(string Reason);
public sealed record ConfigureSystemInstanceRequest(string ConfigurationReference);
