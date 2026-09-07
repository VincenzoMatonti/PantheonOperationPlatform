using Hermes.Domain.Packages.Entities;
using Hermes.Domain.Packages.ValueObjects;

namespace Hermes.Domain.Packages.Repositories;

public interface IMetadataRepository
{
    Task<Metadata?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Metadata?> GetByPackageIdAsync(Guid packageId, CancellationToken cancellationToken = default);

    Task<Metadata?> GetByDataIdAsync(Guid dataId, CancellationToken cancellationToken = default);

    Task<Metadata?> GetByPackageIdAndKeyAsync(Guid packageId, MetadataKey key, CancellationToken cancellationToken = default);

    Task<Metadata?> GetByDataIdAndKeyAsync(Guid dataId, MetadataKey key, CancellationToken cancellationToken = default);

    Task AddAsync(Metadata metadata, CancellationToken cancellationToken = default);

    void Update(Metadata metadata);
}