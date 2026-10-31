using Athena.Domain.Common;

namespace Athena.Domain.Workload.Registry.Exceptions;

public abstract class WorkloadRegistryException(
    WorkloadRegistryErrorCode code,
    string message)
    : DomainException<WorkloadRegistryErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(WorkloadRegistryErrorCode code)
    {
        return code switch
        {
            WorkloadRegistryErrorCode.WorkloadIdRequired
                or WorkloadRegistryErrorCode.WorkloadNameRequired
                or WorkloadRegistryErrorCode.WorkloadNameTooLong
                or WorkloadRegistryErrorCode.BlockIdRequired
                => DomainErrorType.Validation,

            WorkloadRegistryErrorCode.WorkloadNotFound
                => DomainErrorType.NotFound,

            _ => DomainErrorType.Conflict
        };
    }
}

public enum WorkloadRegistryErrorCode
{
    WorkloadIdRequired = 1,
    WorkloadNameRequired = 2,
    WorkloadNameTooLong = 3,
    WorkloadNameAlreadyExists = 4,

    BlockIdRequired = 10,

    WorkloadNotFound = 20,
    WorkloadAlreadyRegistered = 21,
    WorkloadAlreadyUnregistered = 22,
    WorkloadNotRegistered = 23,
    WorkloadAlreadyEnabled = 24,
    WorkloadAlreadyDisabled = 25,
    WorkloadDeletedCannotBeModified = 26,

    WorkloadAlreadyAvailable = 30,
    WorkloadAlreadyUnavailable = 31,
    WorkloadAlreadyAvailabilityUnknown = 32,
    WorkloadAlreadyInError = 33
}