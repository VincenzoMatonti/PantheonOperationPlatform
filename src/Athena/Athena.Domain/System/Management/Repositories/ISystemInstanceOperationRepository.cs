using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.System.Management.Repositories;

public interface ISystemInstanceOperationRepository
{
    Task AddAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default);
    Task AddWithLifecycleAsync(SystemInstanceOperation operation, SystemInstanceLifecycle lifecycle, CancellationToken cancellationToken = default);
    Task SaveWithLifecycleAsync(
        SystemInstanceOperation operation,
        SystemInstanceLifecycle lifecycle,
        SystemInstance? systemInstance = null,
        CancellationToken cancellationToken = default);
    Task UpdateAsync(SystemInstanceOperation operation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstanceOperation>> GetBySystemInstanceIdAsync(SystemInstanceId id, CancellationToken cancellationToken = default);
    Task<bool> HasActiveOperationAsync(SystemInstanceId id, CancellationToken cancellationToken = default);
}
