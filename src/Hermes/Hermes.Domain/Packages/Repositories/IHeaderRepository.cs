using Hermes.Domain.Packages.Entities;
using Hermes.Domain.Packages.ValueObjects;

namespace Hermes.Domain.Packages.Repositories;

public interface IHeaderRepository
{
    Task<Header?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Header?> GetByPackageIdAsync(Guid packageId, CancellationToken cancellationToken = default);

    Task<Header?> GetByPackageIdAndKeyAsync(Guid packageId, HeaderKey key, CancellationToken cancellationToken = default);

    Task AddAsync(Header header, CancellationToken cancellationToken = default);

    void Update(Header header);
}