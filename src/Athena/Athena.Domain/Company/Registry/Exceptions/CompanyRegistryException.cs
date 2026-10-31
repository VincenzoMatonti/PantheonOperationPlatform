using Athena.Domain.Common;

namespace Athena.Domain.Company.Registry.Exceptions;

public abstract class CompanyRegistryException(CompanyRegistryErrorCode code, string message)
    : DomainException<CompanyRegistryErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(CompanyRegistryErrorCode code)
    {
        return code switch
        {
            CompanyRegistryErrorCode.CompanyIdRequired
                or CompanyRegistryErrorCode.CompanyNameRequired
                or CompanyRegistryErrorCode.CompanyNameTooLong
                or CompanyRegistryErrorCode.LegalNameRequired
                or CompanyRegistryErrorCode.LegalNameTooLong
                or CompanyRegistryErrorCode.CountryCodeRequired
                or CompanyRegistryErrorCode.CountryCodeInvalid
                or CompanyRegistryErrorCode.TaxCodeInvalid
                or CompanyRegistryErrorCode.VatNumberInvalid
                or CompanyRegistryErrorCode.RegistrationNumberTooLong
                or CompanyRegistryErrorCode.LegalFormTooLong
                or CompanyRegistryErrorCode.EmailAddressInvalid
                or CompanyRegistryErrorCode.PecAddressInvalid
                or CompanyRegistryErrorCode.PhoneNumberInvalid
                or CompanyRegistryErrorCode.WebsiteUrlInvalid
                or CompanyRegistryErrorCode.PostalAddressInvalid
                or CompanyRegistryErrorCode.GroupIdRequired
                => DomainErrorType.Validation,

            CompanyRegistryErrorCode.CompanyNotFound
                => DomainErrorType.NotFound,

            _ => DomainErrorType.Conflict
        };
    }
}

public enum CompanyRegistryErrorCode
{
    CompanyIdRequired = 1,
    CompanyNameRequired = 2,
    CompanyNameTooLong = 3,
    CompanyNameAlreadyExists = 4,

    LegalNameRequired = 10,
    LegalNameTooLong = 11,

    CountryCodeRequired = 20,
    CountryCodeInvalid = 21,

    TaxCodeInvalid = 30,
    VatNumberInvalid = 31,

    RegistrationNumberTooLong = 40,
    LegalFormTooLong = 41,

    EmailAddressInvalid = 50,
    PecAddressInvalid = 51,
    PhoneNumberInvalid = 52,
    WebsiteUrlInvalid = 53,
    PostalAddressInvalid = 54,

    GroupIdRequired = 60,

    CompanyNotFound = 70,
    CompanyAlreadyRegistered = 71,
    CompanyAlreadyUnregistered = 72,
    CompanyNotRegistered = 73,
    CompanyAlreadyEnabled = 74,
    CompanyAlreadyDisabled = 75,
    CompanyDeletedCannotBeModified = 76
}