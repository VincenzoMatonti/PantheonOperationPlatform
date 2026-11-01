namespace Athena.Cli.Application.Contracts.Ecosystems;

public sealed record SetEcosystemDesiredStateRequest(string DesiredState);
public sealed record EcosystemLifecycleResponse(Guid EcosystemId, string DesiredState, string OperationalStatus, DateTime CreatedAtUtc, DateTime? UpdatedAtUtc, bool IsDeleted);
public sealed record EcosystemOperationResponse(Guid Id, Guid EcosystemId, string Type, string Status, DateTime CreatedAtUtc, DateTime? StartedAtUtc, DateTime? CompletedAtUtc, string? ErrorMessage, bool IsDeleted);
