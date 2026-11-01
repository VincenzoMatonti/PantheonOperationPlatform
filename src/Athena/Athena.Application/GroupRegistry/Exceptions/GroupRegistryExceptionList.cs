namespace Athena.Application.GroupRegistry.Exceptions;

public class GroupNotFoundException(Guid groupId) : GroupRegistryException(GroupRegistryErrorCode.GroupNotFound, $"Group with ID '{groupId}' was not found.")
{
}

public class GroupNotFoundByNameException(string name) : GroupRegistryException(GroupRegistryErrorCode.GroupNotFound, $"Group with name '{name}' was not found.")
{
}

public class GroupNameAlreadyExistsException(string name) : GroupRegistryException(GroupRegistryErrorCode.GroupNameAlreadyExists, $"Group '{name}' already exists.")
{
}