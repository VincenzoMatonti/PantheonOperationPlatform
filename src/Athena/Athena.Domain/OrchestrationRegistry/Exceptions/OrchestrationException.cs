using Athena.Domain.Common;

namespace Athena.Domain.OrchestrationRegistry.Exceptions;

public abstract class OrchestrationException(OrchestrationErrorCode code, string message)
    : DomainException<OrchestrationErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(OrchestrationErrorCode code)
    {
        return code switch
        {
            OrchestrationErrorCode.OrchestrationIdRequired
                or OrchestrationErrorCode.OrchestrationIdInvalid
                or OrchestrationErrorCode.OrchestrationNameRequired
                or OrchestrationErrorCode.InvalidRegistrationStatus
                or OrchestrationErrorCode.InvalidAvailabilityStatus
                or OrchestrationErrorCode.OrchestrationNamesRequired
                => DomainErrorType.Validation,

            _ => DomainErrorType.Conflict
        };
    }
}

public enum OrchestrationErrorCode
{
    OrchestrationIdRequired = 1,
    OrchestrationIdInvalid = 2,
    OrchestrationNameRequired = 3,

    AlreadyRegistered = 10,
    AlreadyUnregistered = 11,
    AlreadyEnabled = 12,
    AlreadyDisabled = 13,
    InvalidRegistrationStatus = 14,

    AlreadyAvailable = 15,
    AlreadyUnavailable = 16,
    AlreadyAvailabilityUnknown = 17,
    InvalidAvailabilityStatus = 18,

    AlreadyDeleted = 20,
    DeletedOrchestrationCannotBeModified = 21,

    NotRegistered = 30,
    NameAlreadyExists = 31,
    OrchestrationNamesRequired = 32
}