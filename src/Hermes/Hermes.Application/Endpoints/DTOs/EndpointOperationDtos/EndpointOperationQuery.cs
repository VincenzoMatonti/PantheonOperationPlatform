namespace Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;

public class GetEndpointOperationByIdQuery
{
    public Guid EndpointOperationId { get; set; }
}

public class GetEndpointOperationQuery
{
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class GetEndpointOperationsByEndpointIdQuery
{
    public Guid EndpointId { get; set; }
}
