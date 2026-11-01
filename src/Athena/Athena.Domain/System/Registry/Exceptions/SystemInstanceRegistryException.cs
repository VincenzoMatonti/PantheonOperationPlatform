using Athena.Domain.Common;

namespace Athena.Domain.System.Registry.Exceptions;

public abstract class SystemInstanceRegistryException(
    SystemInstanceRegistryErrorCode code, string message)
    : DomainException<SystemInstanceRegistryErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(
        SystemInstanceRegistryErrorCode code)
    {
        return code switch
        {
            SystemInstanceRegistryErrorCode.SystemInstanceIdRequired
                or SystemInstanceRegistryErrorCode.SystemInstanceNameRequired
                or SystemInstanceRegistryErrorCode.SystemInstanceNameTooLong
                or SystemInstanceRegistryErrorCode.SystemIdRequired
                or SystemInstanceRegistryErrorCode.WorkloadIdRequired
                => DomainErrorType.Validation,

            SystemInstanceRegistryErrorCode.SystemInstanceNotFound
                => DomainErrorType.NotFound,

            _ => DomainErrorType.Conflict
        };
    }
}

public enum SystemInstanceRegistryErrorCode
{
    SystemInstanceIdRequired = 1,
    SystemInstanceNameRequired = 2,
    SystemInstanceNameTooLong = 3,
    SystemInstanceNameAlreadyExists = 4,

    SystemIdRequired = 10,
    WorkloadIdRequired = 11,

    SystemInstanceNotFound = 20,
    SystemInstanceAlreadyRegistered = 21,
    SystemInstanceAlreadyUnregistered = 22,
    SystemInstanceNotRegistered = 23,
    SystemInstanceAlreadyEnabled = 24,
    SystemInstanceAlreadyDisabled = 25,
    SystemInstanceDisabledCannotBeModified = 26,
    SystemInstanceDeletedCannotBeModified = 27
}