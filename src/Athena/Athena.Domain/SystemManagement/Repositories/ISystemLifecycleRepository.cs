using Athena.Domain.SystemManagement.Entities;
using Athena.Domain.SystemManagement.Enums;
using Athena.Domain.SystemRegistry.ValueObjects;

namespace Athena.Domain.SystemManagement.Repositories;

public interface ISystemLifecycleRepository
{
    Task<SystemLifecycle?> GetBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycle>> GetByDesiredStateAsync(SystemDesiredState desiredState, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemLifecycle>> GetByOperationalStatusAsync(SystemOperationalStatus operationalStatus, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySystemIdAsync(SystemId systemId, CancellationToken cancellationToken = default);
    Task AddAsync(SystemLifecycle lifecycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(SystemLifecycle lifecycle, CancellationToken cancellationToken = default);
}