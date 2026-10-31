using Athena.Domain.Block.Registry.ValueObjects;
using WorkloadEntity = Athena.Domain.Workload.Registry.Entities.Workload;
using Athena.Domain.Workload.Registry.Enums;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Domain.Workload.Registry.Repositories;

public interface IWorkloadRepository
{
    Task<WorkloadEntity?> GetByIdAsync(WorkloadId id, CancellationToken cancellationToken = default);
    Task<WorkloadEntity?> GetByNameAsync(BlockId blockId, WorkloadName name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkloadEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkloadEntity>> GetByBlockIdAsync(BlockId blockId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkloadEntity>> GetByRegistrationStatusAsync(WorkloadRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkloadEntity>> GetByAvailabilityStatusAsync(WorkloadAvailabilityStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkloadEntity>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(WorkloadId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(BlockId blockId, WorkloadName name, CancellationToken cancellationToken = default);
    Task AddAsync(WorkloadEntity workload, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkloadEntity workload, CancellationToken cancellationToken = default);
}