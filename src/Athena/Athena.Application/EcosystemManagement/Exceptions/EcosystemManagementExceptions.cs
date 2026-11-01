using Athena.Application.Common.exceptions;

namespace Athena.Application.EcosystemManagement.Exceptions;

public enum EcosystemManagementErrorCode { InvalidDesiredState = 1, InvalidOperationType = 2, EcosystemLifecycleNotFound = 3, EcosystemLifecycleAlreadyExists = 4 }

public sealed class EcosystemManagementException(EcosystemManagementErrorCode code, string message)
    : ApplicationException<EcosystemManagementErrorCode>(code, "EcosystemManagement", message,
        code is EcosystemManagementErrorCode.InvalidDesiredState or EcosystemManagementErrorCode.InvalidOperationType
            ? ApplicationCoreErrorType.Validation
            : code == EcosystemManagementErrorCode.EcosystemLifecycleNotFound
                ? ApplicationCoreErrorType.NotFound
                : ApplicationCoreErrorType.Conflict);
