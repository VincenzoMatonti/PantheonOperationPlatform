namespace Hermes.Application.Endpoints.Commands;

public class CreateEndpointCommand
{
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
}

public class RenameEndpointCommand
{
    public Guid EndpointId { get; set; }
    public string NewType { get; set; } = null!;
}

public class RenameCodeEndpointCommand
{
    public Guid EndpointId { get; set; }
    public string NewCode { get; set; } = null!;
}

public class ActivateEndpointCommand
{
    public Guid EndpointId { get; set; }
}

public class DeactivateEndpointCommand
{
    public Guid EndpointId { get; set; }
}

public class DeleteEndpointCommand
{
    public Guid EndpointId { get; set; }
}

public class RestoreEndpointCommand
{
    public Guid EndpointId { get; set; }
}