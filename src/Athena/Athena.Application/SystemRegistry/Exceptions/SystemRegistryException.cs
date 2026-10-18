using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemRegistry.Exceptions;

public abstract class SystemRegistryException(SystemRegistryErrorCode code, string message)
    : ApplicationException<SystemRegistryErrorCode>(code, "SystemRegistry", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(SystemRegistryErrorCode code)
    {
        return code switch
        {
            SystemRegistryErrorCode.SystemNotFound
                or SystemRegistryErrorCode.SystemNotFoundByName
                => ApplicationCoreErrorType.NotFound,

            SystemRegistryErrorCode.SystemNameAlreadyExists
                => ApplicationCoreErrorType.Conflict,

            _ => ApplicationCoreErrorType.Internal
        };
    }
}

public enum SystemRegistryErrorCode
{
    SystemNotFound = 1,
    SystemNameAlreadyExists = 2,
    SystemNotFoundByName = 3
}