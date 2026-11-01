using Athena.Domain.Ecosystem.Management.Entities;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Domain.Ecosystem.Management.Repositories;

public interface IEcosystemLifecycleRepository
{
    Task<EcosystemLifecycle?> GetByIdAsync(EcosystemId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemLifecycle>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(EcosystemLifecycle lifecycle, CancellationToken cancellationToken = default);
    Task UpdateAsync(EcosystemLifecycle lifecycle, CancellationToken cancellationToken = default);
}

public interface IEcosystemOperationRepository
{
    Task AddAsync(EcosystemOperation operation, CancellationToken cancellationToken = default);
    Task UpdateAsync(EcosystemOperation operation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemOperation>> GetByEcosystemIdAsync(EcosystemId id, CancellationToken cancellationToken = default);
}
