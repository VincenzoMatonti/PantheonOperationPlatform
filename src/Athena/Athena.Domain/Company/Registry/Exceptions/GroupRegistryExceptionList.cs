namespace Athena.Domain.Company.Registry.Exceptions;

public class GroupIdRequiredException() : GroupRegistryException(
    GroupRegistryErrorCode.GroupIdRequired,
    "Group ID cannot be empty.")
{
}

public class GroupNameRequiredException() : GroupRegistryException(
    GroupRegistryErrorCode.GroupNameRequired,
    "Group name cannot be empty.")
{
}

public class GroupNameTooLongException(int maxLength) : GroupRegistryException(
    GroupRegistryErrorCode.GroupNameTooLong,
    $"Group name cannot exceed {maxLength} characters.")
{
}

public class GroupNameAlreadyExistsException(string groupName) : GroupRegistryException(
    GroupRegistryErrorCode.GroupNameAlreadyExists,
    $"Group '{groupName}' already exists.")
{
}

public class GroupNotFoundException(Guid groupId) : GroupRegistryException(
    GroupRegistryErrorCode.GroupNotFound,
    $"Group with ID '{groupId}' was not found.")
{
}

public class GroupAlreadyRegisteredException(string groupName) : GroupRegistryException(
    GroupRegistryErrorCode.GroupAlreadyRegistered,
    $"Group '{groupName}' is already registered.")
{
}

public class GroupAlreadyUnregisteredException(Guid groupId) : GroupRegistryException(
    GroupRegistryErrorCode.GroupAlreadyUnregistered,
    $"Group with ID '{groupId}' is already unregistered.")
{
}

public class GroupNotRegisteredException(Guid groupId) : GroupRegistryException(
    GroupRegistryErrorCode.GroupNotRegistered,
    $"Group with ID '{groupId}' is not registered.")
{
}

public class GroupAlreadyEnabledException(Guid groupId) : GroupRegistryException(
    GroupRegistryErrorCode.GroupAlreadyEnabled,
    $"Group with ID '{groupId}' is already enabled.")
{
}

public class GroupAlreadyDisabledException(Guid groupId) : GroupRegistryException(
    GroupRegistryErrorCode.GroupAlreadyDisabled,
    $"Group with ID '{groupId}' is already disabled.")
{
}

public class GroupDeletedCannotBeModifiedException(Guid groupId) : GroupRegistryException(
    GroupRegistryErrorCode.GroupDeletedCannotBeModified,
    $"Group with ID '{groupId}' has been deleted and cannot be modified.")
{
}