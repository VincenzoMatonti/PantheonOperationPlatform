using Athena.Domain.Common;

namespace Athena.Domain.System.Management.Exceptions;

public enum SystemInstanceManagementErrorCode
{
    LifecycleDeletedCannotBeModified = 1,
    InvalidDesiredState = 2,
    InvalidOperationalStatus = 3,
    OperationInvalidTransition = 4,
    OperationErrorMessageRequired = 5
}

public sealed class SystemInstanceManagementException(
    SystemInstanceManagementErrorCode code,
    string message,
    DomainErrorType errorType = DomainErrorType.Conflict)
    : DomainException<SystemInstanceManagementErrorCode>(code, message, errorType);
