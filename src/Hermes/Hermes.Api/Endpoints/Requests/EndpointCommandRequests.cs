namespace Hermes.Api.Endpoints.Requests;

public class CreateEndpointRequest
{
    public string Code { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class ChangeEndpointTypeRequest
{
    public Guid EndpointId { get; set; }
    public string NewType { get; set; } = string.Empty;
}

public class RenameCodeEndpointRequest
{
    public Guid EndpointId { get; set; }
    public string NewCode { get; set; } = string.Empty;
}

public class ActivateEndpointRequest
{
    public Guid EndpointId { get; set; }
}

public class DeactivateEndpointRequest
{
    public Guid EndpointId { get; set; }
}

public class DeleteEndpointRequest
{
    public Guid EndpointId { get; set; }
}

public class RestoreEndpointRequest
{
    public Guid EndpointId { get; set; }
}
