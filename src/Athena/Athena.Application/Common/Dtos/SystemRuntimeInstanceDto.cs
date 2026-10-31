namespace Athena.Application.Common.Dtos;

/// <summary>
/// Application representation of a system runtime instance.
/// </summary>
public record SystemRuntimeInstanceDto(
    Guid Id,
    Guid SystemId,
    string RuntimeProvider,
    string RuntimeResourceId,
    string RuntimeResourceName,
    string Environment,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);