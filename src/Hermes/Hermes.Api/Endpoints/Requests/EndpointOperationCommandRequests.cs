namespace Hermes.Api.Endpoints.Requests;

public class CreateEndpointOperationRequest
{
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class EnableEndpointOperationRequest
{
    public Guid EndpointOperationId { get; set; }
}

public class DisableEndpointOperationRequest
{
    public Guid EndpointOperationId { get; set; }
}

public class DeleteEndpointOperationRequest
{
    public Guid EndpointOperationId { get; set; }
}

public class RestoreEndpointOperationRequest
{
    public Guid EndpointOperationId { get; set; }
}
