namespace Athena.Application.BlockManagement.Queries;

public sealed record GetBlockLifecycleQuery(Guid BlockId);
public sealed record GetBlockLifecyclesQuery;
public sealed record GetBlockOperationsQuery(Guid BlockId);
