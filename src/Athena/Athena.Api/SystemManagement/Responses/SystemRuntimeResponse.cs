namespace Athena.Api.SystemManagement.Responses;

public record SystemRuntimeInstanceResponse(
    Guid Id,
    Guid SystemId,
    string RuntimeProvider,
    string RuntimeResourceId,
    string RuntimeResourceName,
    string Environment,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
