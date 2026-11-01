using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.Enums;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.ValueObjects;

namespace Athena.Domain.System.Registry.Repositories;

public interface ISystemInstanceRepository
{
    Task<SystemInstance?> GetByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default);
    Task<SystemInstance?> GetByNameAsync(WorkloadId workloadId, SystemInstanceName name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstance>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstance>> GetBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstance>> GetByWorkloadIdAsync(WorkloadId workloadId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstance>> GetByRegistrationStatusAsync(SystemInstanceRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstance>> GetByOperationalStatusAsync(SystemInstanceOperationalStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstance>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(WorkloadId workloadId, SystemInstanceName name, CancellationToken cancellationToken = default);
    Task AddAsync(SystemInstance systemInstance, CancellationToken cancellationToken = default);
    Task UpdateAsync(SystemInstance systemInstance, CancellationToken cancellationToken = default);
}