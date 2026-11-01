namespace Athena.Application.CompanyRegistry.Exceptions;

public sealed class CompanyNotFoundException(Guid companyId)
    : CompanyRegistryException(
        CompanyRegistryErrorCode.CompanyNotFound,
        $"Company with ID '{companyId}' was not found.");

public sealed class CompanyNotFoundByNameException(string name)
    : CompanyRegistryException(
        CompanyRegistryErrorCode.CompanyNotFound,
        $"Company '{name}' was not found.");

public sealed class CompanyNameAlreadyExistsException(string name)
    : CompanyRegistryException(
        CompanyRegistryErrorCode.CompanyNameAlreadyExists,
        $"Company '{name}' already exists.");

public sealed class CompanyTaxCodeAlreadyExistsException(string taxCode)
    : CompanyRegistryException(
        CompanyRegistryErrorCode.CompanyTaxCodeAlreadyExists,
        $"Company tax code '{taxCode}' already exists.");

public sealed class CompanyVatNumberAlreadyExistsException(string vatNumber)
    : CompanyRegistryException(
        CompanyRegistryErrorCode.CompanyVatNumberAlreadyExists,
        $"Company VAT number '{vatNumber}' already exists.");