using Athena.Domain.Common;

namespace Athena.Domain.Orchestration.Management.Exceptions;

public abstract class OrchestrationSystemInstanceException(OrchestrationSystemInstanceErrorCode code, string message)
    : DomainException<OrchestrationSystemInstanceErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(OrchestrationSystemInstanceErrorCode code)
    {
        return code switch
        {
            OrchestrationSystemInstanceErrorCode.OrchestrationIdRequired or OrchestrationSystemInstanceErrorCode.SystemInstanceIdRequired => DomainErrorType.Validation,
            _ => DomainErrorType.Conflict
        };
    }
}

public enum OrchestrationSystemInstanceErrorCode
{
    OrchestrationIdRequired = 1,
    SystemInstanceIdRequired = 2,

    OrchestrationSystemInstanceAlreadyDeleted = 10,
    OrchestrationSystemInstanceDeletedCannotBeModified = 11,
    OrchestrationSystemInstanceAlreadyRestored = 12
}