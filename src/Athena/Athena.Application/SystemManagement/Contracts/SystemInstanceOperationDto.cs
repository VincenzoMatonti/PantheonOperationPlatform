namespace Athena.Application.SystemManagement.Contracts;

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
    string? ConvergenceStatus = null);

public sealed record ResolveSystemInstanceOperationRequest(string Reason);
