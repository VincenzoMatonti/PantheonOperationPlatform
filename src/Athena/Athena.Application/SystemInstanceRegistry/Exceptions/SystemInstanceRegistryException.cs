using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemInstanceRegistry.Exceptions;

public abstract class SystemInstanceRegistryException(
    SystemInstanceRegistryErrorCode code,
    string message)
    : ApplicationException<SystemInstanceRegistryErrorCode>(
        code,
        "SystemInstanceRegistry",
        message,
        MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(
        SystemInstanceRegistryErrorCode code) => code switch
    {
        SystemInstanceRegistryErrorCode.SystemInstanceNotFound
            or SystemInstanceRegistryErrorCode.SystemInstanceNotFoundByName
                => ApplicationCoreErrorType.NotFound,

        SystemInstanceRegistryErrorCode.SystemInstanceNameAlreadyExists
            => ApplicationCoreErrorType.Conflict,

        SystemInstanceRegistryErrorCode.InvalidSystemInstanceRegistryRequest
            => ApplicationCoreErrorType.Validation,

        _ => ApplicationCoreErrorType.Internal
    };
}

public enum SystemInstanceRegistryErrorCode
{
    SystemInstanceNotFound = 1,
    SystemInstanceNotFoundByName = 2,
    SystemInstanceNameAlreadyExists = 3,
    InvalidSystemInstanceRegistryRequest = 4
}