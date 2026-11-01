using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.System.Management.Repositories;

public interface ISystemInstanceLifecycleRepository
{
    Task<SystemInstanceLifecycle?> GetByIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstanceLifecycle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstanceLifecycle>> GetByDesiredStateAsync(SystemInstanceDesiredState state, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstanceLifecycle>> GetByOperationalStatusAsync(SystemInstanceOperationalStatus status, CancellationToken cancellationToken = default);
    Task AddAsync(SystemInstanceLifecycle lifecycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(SystemInstanceLifecycle lifecycle, CancellationToken cancellationToken = default);
}
