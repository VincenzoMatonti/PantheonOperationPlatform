using Athena.Domain.Common;

namespace Athena.Domain.Ecosystem.Management.Exceptions;

public enum EcosystemManagementErrorCode
{
    LifecycleDeletedCannotBeModified = 1,
    InvalidDesiredState = 2,
    InvalidOperationalStatus = 3,
    OperationInvalidTransition = 4,
    OperationErrorMessageRequired = 5
}

public sealed class EcosystemManagementException(
    EcosystemManagementErrorCode code,
    string message,
    DomainErrorType errorType = DomainErrorType.Conflict)
    : DomainException<EcosystemManagementErrorCode>(code, message, errorType);
