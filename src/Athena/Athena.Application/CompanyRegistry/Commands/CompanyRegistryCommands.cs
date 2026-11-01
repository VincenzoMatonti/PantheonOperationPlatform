namespace Athena.Application.CompanyRegistry.Commands;

public sealed class CreateCompanyCommand
{
    public required string Name { get; init; }
    public required string LegalName { get; init; }
    public required string CountryCode { get; init; }

    public Guid? GroupId { get; init; }
    public string? TaxCode { get; init; }
    public string? VatNumber { get; init; }
    public string? RegistrationNumber { get; init; }
    public string? LegalForm { get; init; }

    public string? Email { get; init; }
    public string? Pec { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Website { get; init; }

    public string? AddressStreet { get; init; }
    public string? AddressCity { get; init; }
    public string? AddressPostalCode { get; init; }
    public string? AddressRegion { get; init; }
    public string? AddressCountryCode { get; init; }
}

public sealed class RegisterCompanyCommand
{
    public required string Name { get; init; }
}

public sealed class UnregisterCompanyCommand
{
    public Guid CompanyId { get; init; }
}

public sealed class RenameCompanyCommand
{
    public Guid CompanyId { get; init; }
    public required string NewName { get; init; }
}

public sealed class ChangeCompanyLegalNameCommand
{
    public Guid CompanyId { get; init; }
    public required string LegalName { get; init; }
}

public sealed class AssignCompanyToGroupCommand
{
    public Guid CompanyId { get; init; }
    public Guid GroupId { get; init; }
}

public sealed class RemoveCompanyFromGroupCommand
{
    public Guid CompanyId { get; init; }
}

public sealed class UpdateCompanyFiscalInformationCommand
{
    public Guid CompanyId { get; init; }
    public required string CountryCode { get; init; }
    public string? TaxCode { get; init; }
    public string? VatNumber { get; init; }
    public string? RegistrationNumber { get; init; }
    public string? LegalForm { get; init; }
}

public sealed class UpdateCompanyContactInformationCommand
{
    public Guid CompanyId { get; init; }
    public string? Email { get; init; }
    public string? Pec { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Website { get; init; }
}

public sealed class UpdateCompanyRegisteredAddressCommand
{
    public Guid CompanyId { get; init; }
    public string? Street { get; init; }
    public string? City { get; init; }
    public string? PostalCode { get; init; }
    public string? Region { get; init; }
    public required string CountryCode { get; init; }
}

public sealed class EnableCompanyCommand
{
    public Guid CompanyId { get; init; }
}

public sealed class DisableCompanyCommand
{
    public Guid CompanyId { get; init; }
}

public sealed class DeleteCompanyCommand
{
    public Guid CompanyId { get; init; }
}