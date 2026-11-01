using Athena.Application.Common.exceptions;

namespace Athena.Application.WorkloadRegistry.Exceptions;

public abstract class WorkloadRegistryException(WorkloadRegistryErrorCode code, string message) : ApplicationException<WorkloadRegistryErrorCode>(code, "WorkloadRegistry", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(WorkloadRegistryErrorCode code) => code switch
    {
        WorkloadRegistryErrorCode.WorkloadNotFound or WorkloadRegistryErrorCode.WorkloadNotFoundByName => ApplicationCoreErrorType.NotFound,
        WorkloadRegistryErrorCode.WorkloadNameAlreadyExists => ApplicationCoreErrorType.Conflict,
        _ => ApplicationCoreErrorType.Internal
    };
}

public enum WorkloadRegistryErrorCode
{
    WorkloadNotFound = 1,
    WorkloadNotFoundByName = 2,
    WorkloadNameAlreadyExists = 3
}