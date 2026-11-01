using Athena.Domain.Company.Entities;

namespace Athena.Application.CompanyRegistry.Contracts;

public static class CompanyRegistryDtoMapper
{
    public static CompanyDto ToDto(Company company)
    {
        return new CompanyDto
        {
            Id = company.Id.Value,
            GroupId = company.GroupId?.Value,

            Name = company.Name.Value,
            LegalName = company.LegalName.Value,
            CountryCode = company.CountryCode.Value,

            TaxCode = company.TaxCode?.Value,
            VatNumber = company.VatNumber?.Value,
            RegistrationNumber = company.RegistrationNumber?.Value,
            LegalForm = company.LegalForm?.Value,

            Email = company.Email?.Value,
            Pec = company.Pec?.Value,
            PhoneNumber = company.PhoneNumber?.Value,
            Website = company.Website?.Value,

            AddressStreet = company.RegisteredAddress?.Street,
            AddressCity = company.RegisteredAddress?.City,
            AddressPostalCode = company.RegisteredAddress?.PostalCode,
            AddressRegion = company.RegisteredAddress?.Region,
            AddressCountryCode = company.RegisteredAddress?.CountryCode.Value,

            RegistrationStatus = company.RegistrationStatus.ToString(),

            CreatedAtUtc = company.CreatedAtUtc,
            UpdatedAtUtc = company.UpdatedAtUtc,
            IsDeleted = company.IsDeleted
        };
    }

    public static IReadOnlyList<CompanyDto> ToDtos(IEnumerable<Company> companies)
    {
        return companies.Select(ToDto).ToList();
    }
}