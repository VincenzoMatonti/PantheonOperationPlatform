namespace Athena.Application.SystemManagement.Contracts;

public sealed record SystemInstanceOperationDto(
    Guid Id,
    Guid SystemInstanceId,
    string Type,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    string? ErrorMessage);
