using Athena.Domain.Common;

namespace Athena.Domain.Workload.Management.Exceptions;

public enum WorkloadManagementErrorCode
{
    LifecycleDeletedCannotBeModified = 1,
    InvalidDesiredState = 2,
    InvalidOperationalStatus = 3,
    OperationInvalidTransition = 4,
    OperationErrorMessageRequired = 5
}

public sealed class WorkloadManagementException(
    WorkloadManagementErrorCode code,
    string message,
    DomainErrorType errorType = DomainErrorType.Conflict)
    : DomainException<WorkloadManagementErrorCode>(code, message, errorType);
