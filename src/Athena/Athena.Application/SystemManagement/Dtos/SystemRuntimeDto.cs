namespace Athena.Application.SystemManagement.Dtos;

public class RegisterSystemRuntimeInstanceCommand
{
    public Guid SystemId { get; set; }

    public string RuntimeProvider { get; set; } = string.Empty;

    public string RuntimeResourceId { get; set; } = string.Empty;

    public string RuntimeResourceName { get; set; } = string.Empty;

    public string Environment { get; set; } = string.Empty;
}

public class UnregisterSystemRuntimeInstanceCommand
{
    public Guid Id { get; set; }
}

public class GetSystemRuntimeInstanceByIdQuery
{
    public Guid Id { get; set; }
}

public class GetSystemRuntimeInstancesBySystemIdQuery
{
    public Guid SystemId { get; set; }
}

public class GetSystemRuntimeInstancesBySystemIdAndEnvironmentQuery
{
    public Guid SystemId { get; set; }

    public string Environment { get; set; } = string.Empty;
}

public class GetSystemRuntimeInstancesByRuntimeResourceNameQuery
{
    public string RuntimeResourceName { get; set; } = string.Empty;
}