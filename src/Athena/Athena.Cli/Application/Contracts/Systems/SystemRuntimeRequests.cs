namespace Athena.Cli.Application.Contracts.Systems;

public sealed record RegisterSystemRuntimeInstanceRequest(
    Guid SystemId,
    string RuntimeProvider,
    string RuntimeResourceId,
    string RuntimeResourceName,
    string Environment);
