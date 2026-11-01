using Athena.Application.Common.exceptions;

namespace Athena.Application.BlockManagement.Exceptions;

public enum BlockManagementErrorCode { InvalidDesiredState = 1, InvalidOperationType = 2, BlockLifecycleNotFound = 3, BlockLifecycleAlreadyExists = 4 }

public sealed class BlockManagementException(BlockManagementErrorCode code, string message)
    : ApplicationException<BlockManagementErrorCode>(code, "BlockManagement", message,
        code is BlockManagementErrorCode.InvalidDesiredState or BlockManagementErrorCode.InvalidOperationType
            ? ApplicationCoreErrorType.Validation
            : code == BlockManagementErrorCode.BlockLifecycleNotFound
                ? ApplicationCoreErrorType.NotFound
                : ApplicationCoreErrorType.Conflict);
