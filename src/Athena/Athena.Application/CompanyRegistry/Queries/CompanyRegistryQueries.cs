using Athena.Application.CompanyRegistry.Enums;

namespace Athena.Application.CompanyRegistry.Queries;

public sealed class GetCompanyByIdQuery
{
    public Guid CompanyId { get; init; }
}

public sealed class GetCompanyByNameQuery
{
    public required string Name { get; init; }
}

public sealed class GetCompaniesByGroupIdQuery
{
    public Guid GroupId { get; init; }
}

public sealed class GetCompaniesByRegistrationStatusQuery
{
    public CompanyRegistrationStatusFilter Status { get; init; }
}

public sealed class GetCompaniesByCountryCodeQuery
{
    public required string CountryCode { get; init; }
}

public sealed class GetCompanyByTaxCodeQuery
{
    public required string CountryCode { get; init; }
    public required string TaxCode { get; init; }
}

public sealed class GetCompanyByVatNumberQuery
{
    public required string CountryCode { get; init; }
    public required string VatNumber { get; init; }
}

public sealed class GetCompaniesByDeletionStatusQuery
{
    public bool IsDeleted { get; init; }
}