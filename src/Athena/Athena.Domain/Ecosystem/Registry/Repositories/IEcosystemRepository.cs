using Athena.Domain.Company.Registry.ValueObjects;
using EcosystemEntity = Athena.Domain.Ecosystem.Registry.Entities.Ecosystem;
using Athena.Domain.Ecosystem.Registry.Enums;
using Athena.Domain.Ecosystem.Registry.ValueObjects;

namespace Athena.Domain.Ecosystem.Registry.Repositories;

public interface IEcosystemRepository
{
    // Retrieval
    Task<EcosystemEntity?> GetByIdAsync(EcosystemId id, CancellationToken cancellationToken = default);
    Task<EcosystemEntity?> GetByNameAsync(EcosystemName name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemEntity>> GetByCompanyIdAsync(CompanyId companyId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemEntity>> GetByRegistrationStatusAsync(EcosystemRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EcosystemEntity>> GetByAvailabilityStatusAsync(EcosystemAvailabilityStatus status, CancellationToken cancellationToken = default);

    // Soft deletion
    Task<IReadOnlyList<EcosystemEntity>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);

    // Existence checks
    Task<bool> ExistsByIdAsync(EcosystemId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(EcosystemName name, CancellationToken cancellationToken = default);

    // Persistence
    Task AddAsync(EcosystemEntity ecosystem, CancellationToken cancellationToken = default);
    Task UpdateAsync(EcosystemEntity ecosystem, CancellationToken cancellationToken = default);
}