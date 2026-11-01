namespace Athena.Application.CompanyRegistry.Contracts;

public sealed class CompanyDto
{
    public Guid Id { get; set; }
    public Guid? GroupId { get; set; }

    public required string Name { get; set; }
    public required string LegalName { get; set; }
    public required string CountryCode { get; set; }

    public string? TaxCode { get; set; }
    public string? VatNumber { get; set; }
    public string? RegistrationNumber { get; set; }
    public string? LegalForm { get; set; }

    public string? Email { get; set; }
    public string? Pec { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Website { get; set; }

    public string? AddressStreet { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressPostalCode { get; set; }
    public string? AddressRegion { get; set; }
    public string? AddressCountryCode { get; set; }

    public required string RegistrationStatus { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}