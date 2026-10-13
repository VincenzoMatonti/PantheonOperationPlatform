using Athena.Domain.Common;

namespace Athena.Domain.SystemManagement.Exceptions;

public abstract class SystemLifecycleException(
    SystemLifecycleErrorCode code,
    string message)
    : DomainException<SystemLifecycleErrorCode>(code, message)
{
}

public enum SystemLifecycleErrorCode
{
    SystemIdRequired = 1,
    InvalidDesiredState = 2,
    DesiredStateAlreadySet = 3,
    AlreadyStarting = 10,
    AlreadyRunning = 11,
    AlreadyStopping = 12,
    AlreadyStopped = 13,
    AlreadyFailed = 14,
    AlreadyUnknown = 15,
    InvalidOperationalTransition = 16,
    AlreadyDeleted = 20,
    DeletedLifecycleCannotBeModified = 21,
    OperationInProgress = 22

}