using Athena.Domain.Workload.Management.Entities;
using Athena.Domain.Workload.Management.Enums;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Domain.Workload.Management.Repositories;

public interface IWorkloadLifecycleRepository
{
    Task<WorkloadLifecycle?> GetByIdAsync(WorkloadId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkloadLifecycle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(WorkloadLifecycle lifecycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkloadLifecycle lifecycle, CancellationToken cancellationToken = default);
}

public interface IWorkloadOperationRepository
{
    Task AddAsync(WorkloadOperation operation, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkloadOperation operation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkloadOperation>> GetByWorkloadIdAsync(WorkloadId id, CancellationToken cancellationToken = default);
}
