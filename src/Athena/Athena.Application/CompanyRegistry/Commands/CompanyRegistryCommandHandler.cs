using Athena.Application.CompanyRegistry.Contracts;
using Athena.Domain.Company.Entities;
using Athena.Domain.Company.Repositories;
using Athena.Domain.Company.ValueObjects;

namespace Athena.Application.CompanyRegistry.Commands;

public sealed class CompanyRegistryCommandHandler(ICompanyRepository companyRepository)
{
    public async Task<CompanyDto> CreateAsync(CreateCompanyCommand command, CancellationToken cancellationToken = default)
    {
        var countryCode = CountryCode.Create(command.CountryCode);
        var groupId = command.GroupId.HasValue ? GroupId.Create(command.GroupId.Value) : null;
        var registeredAddress = CreateAddress(command.AddressStreet, command.AddressCity, command.AddressPostalCode, command.AddressRegion, command.AddressCountryCode ?? command.CountryCode);
        var company = Company.Create(CompanyId.New(), CompanyName.Create(command.Name), LegalName.Create(command.LegalName), countryCode, groupId,
            CreateOptional(command.TaxCode, TaxCode.Create),
            CreateOptional(command.VatNumber, VatNumber.Create),
            CreateOptional(command.RegistrationNumber, RegistrationNumber.Create),
            CreateOptional(command.LegalForm, LegalForm.Create),
            CreateOptional(command.Email, EmailAddress.Create),
            CreateOptional(command.Pec, PecAddress.Create),
            CreateOptional(command.PhoneNumber, PhoneNumber.Create),
            CreateOptional(command.Website, WebsiteUrl.Create),
            registeredAddress);

        await companyRepository.AddAsync(company, cancellationToken);
        return CompanyRegistryDtoMapper.ToDto(company);
    }

    public async Task<CompanyDto?> RegisterAsync(RegisterCompanyCommand command, CancellationToken cancellationToken = default)
    {
        var company = await companyRepository.GetByNameAsync(CompanyName.Create(command.Name), cancellationToken);
        if (company is null) return null;
        company.Register();
        await companyRepository.UpdateAsync(company, cancellationToken);
        return CompanyRegistryDtoMapper.ToDto(company);
    }

    public Task<CompanyDto?> UnregisterAsync(UnregisterCompanyCommand command, CancellationToken cancellationToken = default) => UpdateAsync(command.CompanyId, company => company.Unregister(), cancellationToken);

    public Task<CompanyDto?> RenameAsync(RenameCompanyCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.CompanyId, company => company.Rename(CompanyName.Create(command.NewName)), cancellationToken);

    public Task<CompanyDto?> ChangeLegalNameAsync(ChangeCompanyLegalNameCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.CompanyId, company => company.ChangeLegalName(LegalName.Create(command.LegalName)), cancellationToken);

    public Task<CompanyDto?> AssignToGroupAsync(AssignCompanyToGroupCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.CompanyId, company => company.AssignToGroup(GroupId.Create(command.GroupId)), cancellationToken);

    public Task<CompanyDto?> RemoveFromGroupAsync(RemoveCompanyFromGroupCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.CompanyId, company => company.RemoveFromGroup(), cancellationToken);

    public Task<CompanyDto?> UpdateFiscalInformationAsync(UpdateCompanyFiscalInformationCommand command, CancellationToken cancellationToken = default) => UpdateAsync(command.CompanyId, company =>
        company.UpdateFiscalInformation(CountryCode.Create(command.CountryCode),
                CreateOptional(command.TaxCode, TaxCode.Create),
                CreateOptional(command.VatNumber, VatNumber.Create),
                CreateOptional(command.RegistrationNumber, RegistrationNumber.Create),
                CreateOptional(command.LegalForm, LegalForm.Create)), cancellationToken);

    public Task<CompanyDto?> UpdateContactInformationAsync(UpdateCompanyContactInformationCommand command, CancellationToken cancellationToken = default) => UpdateAsync(command.CompanyId, company =>
        company.UpdateContactInformation(
                CreateOptional(command.Email, EmailAddress.Create),
                CreateOptional(command.Pec, PecAddress.Create),
                CreateOptional(command.PhoneNumber, PhoneNumber.Create),
                CreateOptional(command.Website, WebsiteUrl.Create)), cancellationToken);

    public Task<CompanyDto?> UpdateRegisteredAddressAsync(UpdateCompanyRegisteredAddressCommand command, CancellationToken cancellationToken = default) =>
    UpdateAsync(command.CompanyId, company => company.UpdateRegisteredAddress(CreateAddress(command.Street, command.City, command.PostalCode, command.Region, command.CountryCode)), cancellationToken);

    public Task<CompanyDto?> EnableAsync(EnableCompanyCommand command, CancellationToken cancellationToken = default) => UpdateAsync(command.CompanyId, company => company.Enable(), cancellationToken);

    public Task<CompanyDto?> DisableAsync(DisableCompanyCommand command, CancellationToken cancellationToken = default) => UpdateAsync(command.CompanyId, company => company.Disable(), cancellationToken);

    public Task<CompanyDto?> DeleteAsync(DeleteCompanyCommand command, CancellationToken cancellationToken = default) => UpdateAsync(command.CompanyId, company => company.Delete(), cancellationToken);

    private async Task<CompanyDto?> UpdateAsync(Guid companyId, Action<Company> update, CancellationToken cancellationToken)
    {
        var company = await companyRepository.GetByIdAsync(CompanyId.Create(companyId), cancellationToken);
        if (company is null) return null;
        update(company);
        await companyRepository.UpdateAsync(company, cancellationToken);
        return CompanyRegistryDtoMapper.ToDto(company);
    }

    private static T? CreateOptional<T>(string? value, Func<string, T> factory) where T : class
    {
        return string.IsNullOrWhiteSpace(value) ? null : factory(value);
    }

    private static PostalAddress? CreateAddress(string? street, string? city, string? postalCode, string? region, string? countryCode)
    {
        if (street is null && city is null && postalCode is null && region is null && countryCode is null) return null;
        if (string.IsNullOrWhiteSpace(street) && string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(postalCode) && string.IsNullOrWhiteSpace(region)) return null;
        return PostalAddress.Create(street!, city!, postalCode!, CountryCode.Create(countryCode!), region);
    }
}