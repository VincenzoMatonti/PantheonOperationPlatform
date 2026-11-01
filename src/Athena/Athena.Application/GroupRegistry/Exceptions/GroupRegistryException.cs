using Athena.Application.Common.exceptions;

namespace Athena.Application.GroupRegistry.Exceptions;

public abstract class GroupRegistryException(
    GroupRegistryErrorCode code, string message)
    : ApplicationException<GroupRegistryErrorCode>(code, "GroupRegistry", message, MapErrorType(code))
{
    private static ApplicationCoreErrorType MapErrorType(GroupRegistryErrorCode code) =>
        code switch
        {
            GroupRegistryErrorCode.GroupNotFound => ApplicationCoreErrorType.NotFound,
            GroupRegistryErrorCode.GroupNameAlreadyExists => ApplicationCoreErrorType.Conflict,
            _ => ApplicationCoreErrorType.Internal
        };
}

public enum GroupRegistryErrorCode
{
    GroupNotFound = 1,
    GroupNameAlreadyExists = 2
}