using Athena.Application.CompanyRegistry.Contracts;
using Athena.Application.CompanyRegistry.Enums;
using Athena.Domain.Company.Enums;
using Athena.Domain.Company.Repositories;
using Athena.Domain.Company.ValueObjects;

namespace Athena.Application.CompanyRegistry.Queries;

public sealed class CompanyRegistryQueryHandler(ICompanyRepository companyRepository)
{
    public async Task<CompanyDto?> GetByIdAsync(GetCompanyByIdQuery query, CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.GetByIdAsync(CompanyId.Create(query.CompanyId), cancellationToken);
        return company is null ? null : CompanyRegistryDtoMapper.ToDto(company);
    }

    public async Task<CompanyDto?> GetByNameAsync(GetCompanyByNameQuery query, CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.GetByNameAsync(CompanyName.Create(query.Name), cancellationToken);
        return company is null ? null : CompanyRegistryDtoMapper.ToDto(company);
    }

    public async Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var companies = await companyRepository.GetAllAsync(cancellationToken);
        return CompanyRegistryDtoMapper.ToDtos(companies);
    }

    public async Task<IReadOnlyList<CompanyDto>> GetByGroupIdAsync(GetCompaniesByGroupIdQuery query, CancellationToken cancellationToken = default)
    {
        var companies = await companyRepository.GetByGroupIdAsync(GroupId.Create(query.GroupId), cancellationToken);
        return CompanyRegistryDtoMapper.ToDtos(companies);
    }

    public async Task<IReadOnlyList<CompanyDto>> GetByRegistrationStatusAsync(GetCompaniesByRegistrationStatusQuery query, CancellationToken cancellationToken = default)
    {
        CompanyRegistrationStatus? status = query.Status switch
        {
            CompanyRegistrationStatusFilter.Registered => CompanyRegistrationStatus.Registered,
            CompanyRegistrationStatusFilter.Unregistered => CompanyRegistrationStatus.Unregistered,
            CompanyRegistrationStatusFilter.Disabled => CompanyRegistrationStatus.Disabled,
            _ => null
        };

        if (status is null) return [];
        var companies = await companyRepository.GetByRegistrationStatusAsync(status.Value, cancellationToken);
        return CompanyRegistryDtoMapper.ToDtos(companies);
    }

    public async Task<IReadOnlyList<CompanyDto>> GetByCountryCodeAsync(GetCompaniesByCountryCodeQuery query, CancellationToken cancellationToken = default)
    {
        var companies = await companyRepository.GetByCountryCodeAsync(CountryCode.Create(query.CountryCode), cancellationToken);
        return CompanyRegistryDtoMapper.ToDtos(companies);
    }

    public async Task<CompanyDto?> GetByTaxCodeAsync(GetCompanyByTaxCodeQuery query, CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.GetByTaxCodeAsync(CountryCode.Create(query.CountryCode), TaxCode.Create(query.TaxCode), cancellationToken);
        return company is null ? null : CompanyRegistryDtoMapper.ToDto(company);
    }

    public async Task<CompanyDto?> GetByVatNumberAsync(GetCompanyByVatNumberQuery query, CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.GetByVatNumberAsync(CountryCode.Create(query.CountryCode), VatNumber.Create(query.VatNumber), cancellationToken);
        return company is null ? null : CompanyRegistryDtoMapper.ToDto(company);
    }

    public async Task<IReadOnlyList<CompanyDto>> GetByDeletionStatusAsync(GetCompaniesByDeletionStatusQuery query, CancellationToken cancellationToken = default)
    {
        var companies = await companyRepository.GetByDeletionStatusAsync(query.IsDeleted, cancellationToken);
        return CompanyRegistryDtoMapper.ToDtos(companies);
    }

    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await companyRepository.ExistsByNameAsync(CompanyName.Create(name), cancellationToken);
    }

    public async Task<bool> TaxCodeExistsAsync(string countryCode, string taxCode, CancellationToken cancellationToken = default)
    {
        return await companyRepository.ExistsByTaxCodeAsync(CountryCode.Create(countryCode), TaxCode.Create(taxCode), cancellationToken);
    }

    public async Task<bool> VatNumberExistsAsync(string countryCode, string vatNumber, CancellationToken cancellationToken = default)
    {
        return await companyRepository.ExistsByVatNumberAsync(CountryCode.Create(countryCode), VatNumber.Create(vatNumber), cancellationToken);
    }
}