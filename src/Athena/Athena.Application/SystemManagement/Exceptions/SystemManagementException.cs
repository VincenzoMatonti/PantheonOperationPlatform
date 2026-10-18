using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemManagement.Exceptions;

public abstract class SystemManagementApplicationException(SystemManagementApplicationErrorCode code, string message)
    : ApplicationException<SystemManagementApplicationErrorCode>(code, "SystemManagement", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(SystemManagementApplicationErrorCode code)
    {
        return code switch
        {
            SystemManagementApplicationErrorCode.InvalidDesiredState
                or SystemManagementApplicationErrorCode.InvalidOperationalStatus
                or SystemManagementApplicationErrorCode.InvalidOperationType
                or SystemManagementApplicationErrorCode.InvalidOperationStatus
                => ApplicationCoreErrorType.Validation,

            SystemManagementApplicationErrorCode.SystemNotFound
                or SystemManagementApplicationErrorCode.SystemLifecycleNotFound
                or SystemManagementApplicationErrorCode.SystemLifecycleOperationNotFound
                => ApplicationCoreErrorType.NotFound,

            SystemManagementApplicationErrorCode.SystemLifecycleAlreadyExists
                => ApplicationCoreErrorType.Conflict,

            _ => ApplicationCoreErrorType.Internal
        };
    }
}

public enum SystemManagementApplicationErrorCode
{
    InvalidDesiredState = 1,
    InvalidOperationalStatus = 2,
    InvalidOperationType = 3,
    InvalidOperationStatus = 4,

    SystemNotFound = 5,
    SystemLifecycleAlreadyExists = 6,
    SystemLifecycleNotFound = 7,
    SystemLifecycleOperationNotFound = 8
}