using Athena.Domain.Common;

namespace Athena.Domain.Company.Registry.Exceptions;

public abstract class GroupRegistryException(GroupRegistryErrorCode code, string message)
    : DomainException<GroupRegistryErrorCode>(code, message, MapErrorType(code))
{
    private static DomainErrorType MapErrorType(GroupRegistryErrorCode code)
    {
        return code switch
        {
            GroupRegistryErrorCode.GroupIdRequired
                or GroupRegistryErrorCode.GroupNameRequired
                or GroupRegistryErrorCode.GroupNameTooLong
                => DomainErrorType.Validation,

            GroupRegistryErrorCode.GroupNotFound
                => DomainErrorType.NotFound,

            _ => DomainErrorType.Conflict
        };
    }
}

public enum GroupRegistryErrorCode
{
    GroupIdRequired = 1,
    GroupNameRequired = 2,
    GroupNameTooLong = 3,
    GroupNameAlreadyExists = 4,

    GroupNotFound = 10,
    GroupAlreadyRegistered = 11,
    GroupAlreadyUnregistered = 12,
    GroupNotRegistered = 13,

    GroupAlreadyEnabled = 20,
    GroupAlreadyDisabled = 21,

    GroupDeletedCannotBeModified = 30
}