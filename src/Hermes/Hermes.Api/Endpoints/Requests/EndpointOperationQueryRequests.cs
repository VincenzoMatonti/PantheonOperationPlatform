namespace Hermes.Api.Endpoints.Requests;

public class GetEndpointOperationByIdRequest
{
    public Guid EndpointOperationId { get; set; }
}

public class GetEndpointOperationRequest
{
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class GetEndpointOperationsByEndpointIdRequest
{
    public Guid EndpointId { get; set; }
}

