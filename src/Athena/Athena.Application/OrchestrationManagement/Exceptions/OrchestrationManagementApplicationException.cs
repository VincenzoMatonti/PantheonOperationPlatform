using Athena.Application.Common.exceptions;

namespace Athena.Application.OrchestrationManagement.Exceptions;

public abstract class OrchestrationManagementApplicationException(OrchestrationManagementApplicationErrorCode code, string message)
    : ApplicationException<OrchestrationManagementApplicationErrorCode>(code, "OrchestrationManagement", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(OrchestrationManagementApplicationErrorCode code)
    {
        return code switch
        {
            OrchestrationManagementApplicationErrorCode.InvalidDesiredState
                or OrchestrationManagementApplicationErrorCode.InvalidOperationalStatus
                or OrchestrationManagementApplicationErrorCode.InvalidOperationType
                or OrchestrationManagementApplicationErrorCode.InvalidOperationStatus
                => ApplicationCoreErrorType.Validation,

            OrchestrationManagementApplicationErrorCode.OrchestrationNotFound
                or OrchestrationManagementApplicationErrorCode.OrchestrationLifecycleNotFound
                or OrchestrationManagementApplicationErrorCode.OrchestrationLifecycleOperationNotFound
                => ApplicationCoreErrorType.NotFound,

            OrchestrationManagementApplicationErrorCode.OrchestrationLifecycleAlreadyExists
                or OrchestrationManagementApplicationErrorCode.OrchestrationLifecycleOperationAlreadyExists
                => ApplicationCoreErrorType.Conflict,

            _ => ApplicationCoreErrorType.Internal
        };
    }
}

public enum OrchestrationManagementApplicationErrorCode
{
    InvalidDesiredState = 1,
    InvalidOperationalStatus = 2,
    InvalidOperationType = 3,
    InvalidOperationStatus = 4,

    OrchestrationNotFound = 5,
    OrchestrationLifecycleAlreadyExists = 6,
    OrchestrationLifecycleNotFound = 7,
    OrchestrationLifecycleOperationNotFound = 8,
    OrchestrationLifecycleOperationAlreadyExists = 9
}
