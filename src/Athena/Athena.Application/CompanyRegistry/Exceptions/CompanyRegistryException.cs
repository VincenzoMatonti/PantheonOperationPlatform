using Athena.Application.Common.exceptions;

namespace Athena.Application.CompanyRegistry.Exceptions;

public abstract class CompanyRegistryException(CompanyRegistryErrorCode code, string message)
    : ApplicationException<CompanyRegistryErrorCode>(code, "CompanyRegistry", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(CompanyRegistryErrorCode code) =>
        code switch
        {
            CompanyRegistryErrorCode.CompanyNotFound => ApplicationCoreErrorType.NotFound,
            CompanyRegistryErrorCode.CompanyNameAlreadyExists
                or CompanyRegistryErrorCode.CompanyTaxCodeAlreadyExists
                or CompanyRegistryErrorCode.CompanyVatNumberAlreadyExists =>
                ApplicationCoreErrorType.Conflict,
            _ => ApplicationCoreErrorType.Internal
        };
}

public enum CompanyRegistryErrorCode
{
    CompanyNotFound = 1,
    CompanyNameAlreadyExists = 2,
    CompanyTaxCodeAlreadyExists = 3,
    CompanyVatNumberAlreadyExists = 4
}