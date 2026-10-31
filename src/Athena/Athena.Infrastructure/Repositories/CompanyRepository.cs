using Athena.Domain.Company.Registry.Enums;
using Athena.Domain.Company.Registry.Repositories;
using Athena.Domain.Company.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CompanyEntity = Athena.Domain.Company.Registry.Entities.Company;

namespace Athena.Infrastructure.Repositories;

public class CompanyRepository(AthenaDbContext dbContext) : ICompanyRepository
{
    public async Task<CompanyEntity?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await dbContext.Companies.FirstOrDefaultAsync(company => company.Id == id && !company.IsDeleted, cancellationToken);
    }

    public async Task<CompanyEntity?> GetByNameAsync(CompanyName name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        return await dbContext.Companies.FirstOrDefaultAsync(company => company.Name == name && !company.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<CompanyEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Companies.Where(company => !company.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CompanyEntity>> GetByGroupIdAsync(GroupId groupId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(groupId);
        return await dbContext.Companies.Where(company => company.GroupId == groupId && !company.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CompanyEntity>> GetByRegistrationStatusAsync(CompanyRegistrationStatus registrationStatus, CancellationToken cancellationToken = default)
    {
        return await dbContext.Companies.Where(company => company.RegistrationStatus == registrationStatus && !company.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CompanyEntity>> GetByCountryCodeAsync(CountryCode countryCode, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(countryCode);
        return await dbContext.Companies.Where(company => company.CountryCode == countryCode && !company.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<CompanyEntity?> GetByTaxCodeAsync(CountryCode countryCode, TaxCode taxCode, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(countryCode);
        ArgumentNullException.ThrowIfNull(taxCode);
        return await dbContext.Companies.FirstOrDefaultAsync(company => company.CountryCode == countryCode && company.TaxCode == taxCode && !company.IsDeleted, cancellationToken);
    }

    public async Task<CompanyEntity?> GetByVatNumberAsync(CountryCode countryCode, VatNumber vatNumber, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(countryCode);
        ArgumentNullException.ThrowIfNull(vatNumber);
        return await dbContext.Companies.FirstOrDefaultAsync(company => company.CountryCode == countryCode && company.VatNumber == vatNumber && !company.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<CompanyEntity>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default)
    {
        return await dbContext.Companies.Where(company => company.IsDeleted == isDeleted).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(CompanyId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await dbContext.Companies.AnyAsync(company => company.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(CompanyName name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        return await dbContext.Companies.AnyAsync(company => company.Name == name, cancellationToken);
    }

    public async Task<bool> ExistsByTaxCodeAsync(CountryCode countryCode, TaxCode taxCode, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(countryCode);
        ArgumentNullException.ThrowIfNull(taxCode);
        return await dbContext.Companies.AnyAsync(company => company.CountryCode == countryCode && company.TaxCode == taxCode, cancellationToken);
    }

    public async Task<bool> ExistsByVatNumberAsync(CountryCode countryCode, VatNumber vatNumber, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(countryCode);
        ArgumentNullException.ThrowIfNull(vatNumber);
        return await dbContext.Companies.AnyAsync(company => company.CountryCode == countryCode && company.VatNumber == vatNumber, cancellationToken);
    }

    public async Task AddAsync(CompanyEntity company, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(company);
        await dbContext.Companies.AddAsync(company, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CompanyEntity company, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(company);
        if (dbContext.Entry(company).State == EntityState.Detached) dbContext.Companies.Update(company);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
