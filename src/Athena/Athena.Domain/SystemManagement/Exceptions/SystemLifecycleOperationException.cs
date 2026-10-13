using Athena.Domain.Common;

namespace Athena.Domain.SystemManagement.Exceptions;

public abstract class SystemLifecycleOperationException(
    SystemLifecycleOperationErrorCode code,
    string message)
    : DomainException<SystemLifecycleOperationErrorCode>(code, message)
{
}

public enum SystemLifecycleOperationErrorCode
{
    SystemIdRequired = 1,
    InvalidOperationType = 2,
    ErrorMessageRequired = 3,

    OperationNotPending = 10,
    OperationNotRunning = 11,

    OperationAlreadyRunning = 20,
    OperationAlreadyCompleted = 21,
    OperationAlreadyFailed = 22,
    OperationAlreadyCancelled = 23,
    OperationAlreadyDeleted = 24,

    DeletedOperationCannotBeModified = 30,
    OperationInProgress = 31
}