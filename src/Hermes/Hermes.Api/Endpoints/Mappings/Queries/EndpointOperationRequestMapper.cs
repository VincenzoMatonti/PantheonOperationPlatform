using Hermes.Api.Endpoints.Requests;
using Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;

namespace Hermes.Api.Endpoints.Mappings.Queries;

public class EndpointOperationQueryRequestMapping
{
    public GetEndpointOperationByIdQuery ToQuery(GetEndpointOperationByIdRequest request)
    {
        return new GetEndpointOperationByIdQuery
        {
            EndpointOperationId = request.EndpointOperationId
        };
    }

    public GetEndpointOperationQuery ToQuery(GetEndpointOperationRequest request)
    {
        return new GetEndpointOperationQuery
        {
            EndpointId = request.EndpointId,
            OperationTypeId = request.OperationTypeId
        };
    }

    public GetEndpointOperationsByEndpointIdQuery ToQuery(GetEndpointOperationsByEndpointIdRequest request)
    {
        return new GetEndpointOperationsByEndpointIdQuery
        {
            EndpointId = request.EndpointId
        };
    }
}