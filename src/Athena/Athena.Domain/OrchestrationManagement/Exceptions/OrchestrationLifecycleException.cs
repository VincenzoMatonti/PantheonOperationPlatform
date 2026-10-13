
using Athena.Domain.Common;

namespace Athena.Domain.OrchestrationManagement.Exceptions;

public abstract class OrchestrationLifecycleException(
    OrchestrationLifecycleErrorCode code,
    string message)
    : DomainException<OrchestrationLifecycleErrorCode>(code, message)
{
}

public enum OrchestrationLifecycleErrorCode
{
    OrchestrationIdRequired = 1,
    OrchestrationLifecycleAlreadyDeleted = 2,
    OrchestrationLifecycleDeletedCannotBeModified = 3,

    DesiredStateAlreadySet = 10,
    AlreadyStarting = 11,
    AlreadyRunning = 12,
    AlreadyStopping = 13,
    AlreadyStopped = 14,
    AlreadyFailed = 15,
    AlreadyOperationalStatusUnknown = 16,

    InvalidDesiredState = 20,
    InvalidOperationalStatus = 21
}
