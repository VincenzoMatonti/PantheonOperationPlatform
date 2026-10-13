
using Athena.Domain.Common;

namespace Athena.Domain.OrchestrationManagement.Exceptions;

public abstract class OrchestrationLifecycleOperationException(
    OrchestrationLifecycleOperationErrorCode code,
    string message)
    : DomainException<OrchestrationLifecycleOperationErrorCode>(code, message)
{
}

public enum OrchestrationLifecycleOperationErrorCode
{
    IdRequired = 1,
    OrchestrationIdRequired = 2,
    ErrorMessageRequired = 3,

    InvalidType = 10,
    InvalidStatus = 11,

    AlreadyRunning = 20,
    AlreadyCompleted = 21,
    AlreadyFailed = 22,
    AlreadyCancelled = 23,
    AlreadyDeleted = 24,

    DeletedCannotBeModified = 30,

    CannotStartFromCurrentStatus = 40,
    CannotCompleteFromCurrentStatus = 41,
    CannotFailFromCurrentStatus = 42,
    CannotCancelFromCurrentStatus = 43
}