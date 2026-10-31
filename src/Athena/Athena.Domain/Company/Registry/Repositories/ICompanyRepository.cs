using CompanyEntity = Athena.Domain.Company.Registry.Entities.Company;
using Athena.Domain.Company.Registry.Enums;
using Athena.Domain.Company.Registry.ValueObjects;

namespace Athena.Domain.Company.Registry.Repositories;

public interface ICompanyRepository
{
    // Retrieval
    Task<CompanyEntity?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken = default);
    Task<CompanyEntity?> GetByNameAsync(CompanyName name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CompanyEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CompanyEntity>> GetByGroupIdAsync(GroupId groupId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CompanyEntity>> GetByRegistrationStatusAsync(CompanyRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CompanyEntity>> GetByCountryCodeAsync(CountryCode countryCode, CancellationToken cancellationToken = default);
    Task<CompanyEntity?> GetByTaxCodeAsync(CountryCode countryCode, TaxCode taxCode, CancellationToken cancellationToken = default);
    Task<CompanyEntity?> GetByVatNumberAsync(CountryCode countryCode, VatNumber vatNumber, CancellationToken cancellationToken = default);

    // Soft deletion
    Task<IReadOnlyList<CompanyEntity>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);

    // Existence checks
    Task<bool> ExistsByIdAsync(CompanyId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(CompanyName name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTaxCodeAsync(CountryCode countryCode, TaxCode taxCode, CancellationToken cancellationToken = default);
    Task<bool> ExistsByVatNumberAsync(CountryCode countryCode, VatNumber vatNumber, CancellationToken cancellationToken = default);

    // Persistence
    Task AddAsync(CompanyEntity company, CancellationToken cancellationToken = default);
    Task UpdateAsync(CompanyEntity company, CancellationToken cancellationToken = default);
}