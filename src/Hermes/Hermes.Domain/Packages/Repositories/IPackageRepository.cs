using Hermes.Domain.Packages.Entities;

namespace Hermes.Domain.Packages.Repositories;

public interface IPackageRepository
{
    Task<Package?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Package>> GetByOperationIdAsync(Guid operationId, CancellationToken cancellationToken = default);

    Task AddAsync(Package package, CancellationToken cancellationToken = default);

    void Update(Package package);
}