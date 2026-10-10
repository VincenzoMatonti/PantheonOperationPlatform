using Athena.Domain.SystemManagement.Entities;
using Athena.Domain.SystemManagement.Enums;
using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Domain.SystemManagement.Repositories;

public interface ISystemLifecycleOperationRepository
{
    Task<SystemLifecycleOperation?> GetByIdAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperation>> GetBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperation>> GetByStatusAsync(SystemLifecycleOperationStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycleOperation>> GetBySystemIdAndStatusAsync(SystemId systemId, SystemLifecycleOperationStatus status, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(Guid operationId, CancellationToken cancellationToken = default);
    Task AddAsync(SystemLifecycleOperation operation, CancellationToken cancellationToken = default);
    Task UpdateAsync(SystemLifecycleOperation operation, CancellationToken cancellationToken = default);
}