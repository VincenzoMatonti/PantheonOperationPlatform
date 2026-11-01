using Athena.Application.Common.exceptions;

namespace Athena.Application.WorkloadManagement.Exceptions;

public enum WorkloadManagementErrorCode { InvalidDesiredState = 1, InvalidOperationType = 2, WorkloadLifecycleNotFound = 3, WorkloadLifecycleAlreadyExists = 4 }

public sealed class WorkloadManagementException(WorkloadManagementErrorCode code, string message)
    : ApplicationException<WorkloadManagementErrorCode>(code, "WorkloadManagement", message,
        code is WorkloadManagementErrorCode.InvalidDesiredState or WorkloadManagementErrorCode.InvalidOperationType
            ? ApplicationCoreErrorType.Validation
            : code == WorkloadManagementErrorCode.WorkloadLifecycleNotFound
                ? ApplicationCoreErrorType.NotFound
                : ApplicationCoreErrorType.Conflict);
