using Hermes.Domain.Packages.Entities;

namespace Hermes.Domain.Packages.Repositories;

public interface IDataRepository
{
    Task<Data?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Data>> GetByPackageIdAsync(Guid packageId, CancellationToken cancellationToken = default);

    Task AddAsync(Data data, CancellationToken cancellationToken = default);

    void Update(Data data);
}