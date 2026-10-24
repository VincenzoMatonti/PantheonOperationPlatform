namespace Athena.Cli.Application.Contracts.Systems;

public sealed record SystemRuntimeInstanceResponse(
    Guid Id,
    Guid SystemId,
    string RuntimeProvider,
    string RuntimeResourceId,
    string RuntimeResourceName,
    string Environment,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
