using Hermes.Api.Clients.Requests;
using Hermes.Application.Clients.DTOs.ClientDTOs;

namespace Hermes.Api.Clients.Mappings.Queries;

public class ClientQueryRequestMapper
{
    public GetClientByIdQuery ToQuery(GetClientByIdRequest request)
    {
        return new GetClientByIdQuery
        {
            ClientId = request.ClientId
        };
    }

    public GetClientByCodeQuery ToQuery(GetClientByCodeRequest request)
    {
        return new GetClientByCodeQuery
        {
            Code = request.Code
        };
    }

    public GetClientCodeByIdQuery ToQuery(GetClientCodeByIdRequest request)
    {
        return new GetClientCodeByIdQuery
        {
            ClientId = request.ClientId
        };
    }
}

