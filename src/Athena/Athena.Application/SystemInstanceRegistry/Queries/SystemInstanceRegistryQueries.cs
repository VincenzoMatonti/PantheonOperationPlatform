using Athena.Domain.System.Registry.Enums;

namespace Athena.Application.SystemInstanceRegistry.Queries;


public sealed record GetSystemInstanceByIdQuery(Guid SystemInstanceId);
public sealed record GetSystemInstanceByNameQuery(Guid WorkloadId, string Name);
public sealed record GetSystemInstancesBySystemIdQuery(Guid SystemId);
public sealed record GetSystemInstancesByWorkloadIdQuery(Guid WorkloadId);
public sealed record GetSystemInstancesByRegistrationStatusQuery(SystemInstanceRegistrationStatus Status);
public sealed record GetSystemInstancesByDeletionStatusQuery(bool IsDeleted);
public sealed record SystemInstanceExistsByIdQuery(Guid SystemInstanceId);
public sealed record SystemInstanceExistsByNameQuery(Guid WorkloadId, string Name);