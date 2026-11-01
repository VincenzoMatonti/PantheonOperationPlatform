namespace Athena.Application.WorkloadRegistry.Queries;

public sealed record GetWorkloadByIdQuery(Guid Id);
public sealed record GetWorkloadByNameQuery(Guid BlockId, string Name);
public sealed record GetWorkloadsByBlockIdQuery(Guid BlockId);
public sealed record GetWorkloadsByRegistrationStatusQuery(int Status);
public sealed record GetWorkloadsByAvailabilityStatusQuery(int Status);
public sealed record GetWorkloadsByDeletionStatusQuery(bool IsDeleted);