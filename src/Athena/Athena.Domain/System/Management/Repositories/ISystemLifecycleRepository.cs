using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.System.Management.Repositories;

public interface ISystemLifecycleRepository
{
    // Retrieval
    Task<SystemLifecycle?> GetBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycle>> GetByDesiredStateAsync(SystemDesiredState desiredState, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycle>> GetByOperationalStatusAsync(SystemOperationalStatus operationalStatus, CancellationToken cancellationToken = default);

    // Soft deletion
    Task<IReadOnlyList<SystemLifecycle>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);

    // Existence checks
    Task<bool> ExistsBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default);

    // Persistence
    Task AddAsync(SystemLifecycle lifecycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(SystemLifecycle lifecycle, CancellationToken cancellationToken = default);
}
