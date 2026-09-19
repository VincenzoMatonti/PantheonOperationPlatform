using Hermes.Api.Endpoints.Requests;
using Hermes.Application.Endpoints.Queries;

namespace Hermes.Api.Endpoints.Mappings.Queries;

public class EndpointQueryRequestMapper
{
    public GetEndpointByIdQuery ToQuery(GetEndpointByIdRequest request)
    {
        return new GetEndpointByIdQuery
        {
            EndpointId = request.EndpointId
        };
    }

    public GetEndpointByCodeQuery ToQuery(GetEndpointByCodeRequest request)
    {
        return new GetEndpointByCodeQuery
        {
            Code = request.Code
        };
    }

    public GetEndpointByTypeQuery ToQuery(GetEndpointByTypeRequest request)
    {
        return new GetEndpointByTypeQuery
        {
            Type = request.Type
        };
    }

    public GetEndpointCodeByIdQuery ToQuery(GetEndpointCodeByIdRequest request)
    {
        return new GetEndpointCodeByIdQuery
        {
            EndpointId = request.EndpointId
        };
    }
}
