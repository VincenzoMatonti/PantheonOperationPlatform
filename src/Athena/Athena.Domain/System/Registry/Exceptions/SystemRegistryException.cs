
using Athena.Domain.Common;

namespace Athena.Domain.System.Registry.Exceptions;

public abstract class SystemRegistryException(SystemRegistryErrorCode code, string message)
    : DomainException<SystemRegistryErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(SystemRegistryErrorCode code)
    {
        return code switch
        {
            SystemRegistryErrorCode.SystemIdRequired
                or SystemRegistryErrorCode.SystemNameRequired
                or SystemRegistryErrorCode.SystemNameTooLong
                or SystemRegistryErrorCode.SystemEndpointRequired
                or SystemRegistryErrorCode.SystemEndpointInvalid
                or SystemRegistryErrorCode.SystemCapabilityRequired
                => DomainErrorType.Validation,

            SystemRegistryErrorCode.SystemNotFound
                => DomainErrorType.NotFound,

            _ => DomainErrorType.Conflict
        };
    }
}

public enum SystemRegistryErrorCode
{
    SystemIdRequired = 1,
    SystemNameRequired = 2,
    SystemNameTooLong = 3,
    SystemNameAlreadyExists = 4,

    SystemEndpointRequired = 10,
    SystemEndpointInvalid = 11,
    SystemEndpointAlreadyExists = 12,

    SystemCapabilityRequired = 20,
    SystemCapabilityAlreadyExists = 21,

    SystemAlreadyEnabled = 30,
    SystemAlreadyDisabled = 31,

    SystemNotFound = 40,
    SystemAlreadyRegistered = 41,
    SystemAlreadyUnregistered = 42,
    SystemNotRegistered = 43,

    SystemAlreadyAvailable = 50,
    SystemAlreadyUnavailable = 51,
    SystemAlreadyAvailabilityUnknown = 52,
    SystemDisabledCannotBeModified = 53,
    SystemDeletedCannotBeModified = 54
}