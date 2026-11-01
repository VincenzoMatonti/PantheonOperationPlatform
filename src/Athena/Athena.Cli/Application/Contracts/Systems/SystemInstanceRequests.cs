namespace Athena.Cli.Application.Contracts.Systems;

public sealed record CreateSystemInstanceRequest(
    Guid SystemInstanceId,
    Guid? SystemId,
    Guid? WorkloadId,
    string? Name);

public sealed record RegisterSystemInstanceRequest(Guid SystemInstanceId);
