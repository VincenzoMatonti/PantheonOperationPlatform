namespace Athena.Domain.Company.Registry.Exceptions;

public class CompanyIdRequiredException() : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyIdRequired,
    "Company ID cannot be empty.")
{
}

public class CompanyNameRequiredException() : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyNameRequired,
    "Company name cannot be empty.")
{
}

public class CompanyNameTooLongException(int maxLength) : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyNameTooLong,
    $"Company name cannot exceed {maxLength} characters.")
{
}

public class CompanyNameAlreadyExistsException(string companyName) : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyNameAlreadyExists,
    $"Company '{companyName}' already exists.")
{
}

public class LegalNameRequiredException() : CompanyRegistryException(
    CompanyRegistryErrorCode.LegalNameRequired,
    "Company legal name cannot be empty.")
{
}

public class LegalNameTooLongException(int maxLength) : CompanyRegistryException(
    CompanyRegistryErrorCode.LegalNameTooLong,
    $"Company legal name cannot exceed {maxLength} characters.")
{
}

public class CountryCodeRequiredException() : CompanyRegistryException(
    CompanyRegistryErrorCode.CountryCodeRequired,
    "Company country code cannot be empty.")
{
}

public class CountryCodeInvalidException(string countryCode) : CompanyRegistryException(
    CompanyRegistryErrorCode.CountryCodeInvalid,
    $"Country code '{countryCode}' must be a valid ISO 3166-1 alpha-2 code.")
{
}

public class TaxCodeInvalidException() : CompanyRegistryException(
    CompanyRegistryErrorCode.TaxCodeInvalid,
    "Company tax identification number is invalid.")
{
}

public class VatNumberInvalidException() : CompanyRegistryException(
    CompanyRegistryErrorCode.VatNumberInvalid,
    "Company VAT number is invalid.")
{
}

public class RegistrationNumberTooLongException(int maxLength) : CompanyRegistryException(
    CompanyRegistryErrorCode.RegistrationNumberTooLong,
    $"Company registration number cannot exceed {maxLength} characters.")
{
}

public class LegalFormTooLongException(int maxLength) : CompanyRegistryException(
    CompanyRegistryErrorCode.LegalFormTooLong,
    $"Company legal form cannot exceed {maxLength} characters.")
{
}

public class EmailAddressInvalidException() : CompanyRegistryException(
    CompanyRegistryErrorCode.EmailAddressInvalid,
    "Company email address is invalid.")
{
}

public class PecAddressInvalidException() : CompanyRegistryException(
    CompanyRegistryErrorCode.PecAddressInvalid,
    "Company PEC address is invalid.")
{
}

public class PhoneNumberInvalidException() : CompanyRegistryException(
    CompanyRegistryErrorCode.PhoneNumberInvalid,
    "Company phone number is invalid.")
{
}

public class WebsiteUrlInvalidException() : CompanyRegistryException(
    CompanyRegistryErrorCode.WebsiteUrlInvalid,
    "Company website URL must be an absolute HTTP or HTTPS URL.")
{
}

public class PostalAddressInvalidException() : CompanyRegistryException(
    CompanyRegistryErrorCode.PostalAddressInvalid,
    "Company registered address is invalid.")
{
}

public class CompanyNotFoundException(Guid companyId) : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyNotFound,
    $"Company with ID '{companyId}' was not found.")
{
}

public class CompanyAlreadyRegisteredException(string companyName) : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyAlreadyRegistered,
    $"Company '{companyName}' is already registered.")
{
}

public class CompanyAlreadyUnregisteredException(Guid companyId) : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyAlreadyUnregistered,
    $"Company with ID '{companyId}' is already unregistered.")
{
}

public class CompanyNotRegisteredException(Guid companyId) : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyNotRegistered,
    $"Company with ID '{companyId}' is not registered.")
{
}

public class CompanyAlreadyEnabledException(Guid companyId) : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyAlreadyEnabled,
    $"Company with ID '{companyId}' is already enabled.")
{
}

public class CompanyAlreadyDisabledException(Guid companyId) : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyAlreadyDisabled,
    $"Company with ID '{companyId}' is already disabled.")
{
}

public class CompanyDeletedCannotBeModifiedException(Guid companyId) : CompanyRegistryException(
    CompanyRegistryErrorCode.CompanyDeletedCannotBeModified,
    $"Company with ID '{companyId}' has been deleted and cannot be modified.")
{
}