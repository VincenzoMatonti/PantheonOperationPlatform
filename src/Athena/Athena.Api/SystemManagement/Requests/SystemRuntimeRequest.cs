namespace Athena.Api.SystemManagement.Requests;

public class GetSystemRuntimeInstanceByIdRequest
{
    public Guid Id { get; set; }
}

public class GetSystemRuntimeInstancesBySystemIdRequest
{
    public Guid SystemId { get; set; }
}

public class GetSystemRuntimeInstancesBySystemIdAndEnvironmentRequest
{
    public Guid SystemId { get; set; }

    public string Environment { get; set; } = string.Empty;
}

public class GetSystemRuntimeInstancesByRuntimeResourceNameRequest
{
    public string RuntimeResourceName { get; set; } = string.Empty;
}

public class RegisterSystemRuntimeInstanceRequest
{
    public Guid SystemId { get; set; }

    public string RuntimeProvider { get; set; } = string.Empty;

    public string RuntimeResourceId { get; set; } = string.Empty;

    public string RuntimeResourceName { get; set; } = string.Empty;

    public string Environment { get; set; } = string.Empty;
}

public class UnregisterSystemRuntimeInstanceRequest
{
    public Guid Id { get; set; }
}
