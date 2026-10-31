using Athena.Domain.Common;

namespace Athena.Domain.Ecosystem.Registry.Exceptions;

public abstract class EcosystemRegistryException(
    EcosystemRegistryErrorCode code,
    string message)
    : DomainException<EcosystemRegistryErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(EcosystemRegistryErrorCode code)
    {
        return code switch
        {
            EcosystemRegistryErrorCode.EcosystemIdRequired
                or EcosystemRegistryErrorCode.EcosystemNameRequired
                or EcosystemRegistryErrorCode.EcosystemNameTooLong
                or EcosystemRegistryErrorCode.CompanyIdRequired
                => DomainErrorType.Validation,

            EcosystemRegistryErrorCode.EcosystemNotFound
                => DomainErrorType.NotFound,

            _ => DomainErrorType.Conflict
        };
    }
}

public enum EcosystemRegistryErrorCode
{
    EcosystemIdRequired = 1,
    EcosystemNameRequired = 2,
    EcosystemNameTooLong = 3,
    EcosystemNameAlreadyExists = 4,

    CompanyIdRequired = 10,

    EcosystemNotFound = 20,
    EcosystemAlreadyRegistered = 21,
    EcosystemAlreadyUnregistered = 22,
    EcosystemNotRegistered = 23,
    EcosystemAlreadyEnabled = 24,
    EcosystemAlreadyDisabled = 25,
    EcosystemDeletedCannotBeModified = 26,

    EcosystemAlreadyAvailable = 30,
    EcosystemAlreadyUnavailable = 31,
    EcosystemAlreadyAvailabilityUnknown = 32,
    EcosystemAlreadyInError = 33
}