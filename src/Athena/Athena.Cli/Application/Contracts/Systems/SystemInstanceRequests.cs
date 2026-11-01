namespace Athena.Cli.Application.Contracts.Systems;

public sealed record CreateSystemInstanceRequest(
    Guid SystemInstanceId,
    Guid? SystemId,
    Guid? WorkloadId,
    string? Name,
    string? ConfigurationReference = null);

public sealed record RegisterSystemInstanceRequest(Guid SystemInstanceId, string? ConfigurationReference = null);
public sealed record ConfigureSystemInstanceRequest(string ConfigurationReference);
