using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Registry.ValueObjects;

namespace Athena.Domain.System.Management.Repositories;

public interface ISystemLifecycleOperationRepository
{
    // Retrieval
    Task<SystemLifecycleOperation?> GetByIdAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperation>> GetBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperation>> GetByStatusAsync(SystemLifecycleOperationStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperation>> GetBySystemIdAndStatusAsync(SystemId systemId, SystemLifecycleOperationStatus status, CancellationToken cancellationToken = default);

    // Soft deletion
    Task<IReadOnlyList<SystemLifecycleOperation>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);

    // Existence checks
    Task<bool> ExistsByIdAsync(Guid operationId, CancellationToken cancellationToken = default);

    // Persistence
    Task AddAsync(SystemLifecycleOperation operation, CancellationToken cancellationToken = default);
    Task UpdateAsync(SystemLifecycleOperation operation, CancellationToken cancellationToken = default);
}
