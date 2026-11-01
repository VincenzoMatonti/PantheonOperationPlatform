namespace Athena.Application.EcosystemManagement.Queries;

public sealed record GetEcosystemLifecycleQuery(Guid EcosystemId);
public sealed record GetEcosystemLifecyclesQuery;
public sealed record GetEcosystemOperationsQuery(Guid EcosystemId);
