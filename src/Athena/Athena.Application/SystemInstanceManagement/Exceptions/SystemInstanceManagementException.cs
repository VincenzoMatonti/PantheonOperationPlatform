using Athena.Application.Common.exceptions;

namespace Athena.Application.SystemInstanceManagement.Exceptions;

public abstract class SystemInstanceManagementException(SystemInstanceManagementErrorCode code, string message)
    : ApplicationException<SystemInstanceManagementErrorCode>(code, "SystemInstanceManagement", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(SystemInstanceManagementErrorCode code) => code switch
    {
        SystemInstanceManagementErrorCode.SystemInstanceNotFound or SystemInstanceManagementErrorCode.SystemInstanceLifecycleNotFound or SystemInstanceManagementErrorCode.SystemInstanceOperationNotFound => ApplicationCoreErrorType.NotFound,
        SystemInstanceManagementErrorCode.SystemInstanceOperationAlreadyActive => ApplicationCoreErrorType.Conflict,
        _ => ApplicationCoreErrorType.Internal
    };
}

public enum SystemInstanceManagementErrorCode
{
    SystemInstanceNotFound = 1,
    SystemInstanceLifecycleNotFound = 2,
    SystemInstanceOperationNotFound = 3,
    SystemInstanceOperationAlreadyActive = 4,
    InvalidSystemInstanceRequest = 5,
    InvalidSystemInstanceOperation = 6,
    InvalidSystemInstanceLogTail = 7
}