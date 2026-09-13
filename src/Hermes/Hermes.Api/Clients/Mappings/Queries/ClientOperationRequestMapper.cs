using Hermes.Api.Clients.Requests;
using Hermes.Application.Clients.DTOs.ClientOperationDtos;

namespace Hermes.Api.Clients.Mappings.Queries;

public class ClientOperationQueryRequestMapper
{
    public GetClientOperationByIdQuery ToQuery(GetClientOperationByIdRequest request)
    {
        return new GetClientOperationByIdQuery
        {
            ClientOperationId = request.ClientOperationId
        };
    }

    public GetClientOperationQuery ToQuery(GetClientOperationRequest request)
    {
        return new GetClientOperationQuery
        {
            ClientId = request.ClientId,
            OperationTypeId = request.OperationTypeId
        };
    }

    public GetClientOperationsByClientIdQuery ToQuery(GetClientOperationsByClientIdRequest request)
    {
        return new GetClientOperationsByClientIdQuery
        {
            ClientId = request.ClientId
        };
    }

    public GetClientOperationsByOperationTypeIdQuery ToQuery(GetClientOperationsByOperationTypeIdRequest request)
    {
        return new GetClientOperationsByOperationTypeIdQuery
        {
            OperationTypeId = request.OperationTypeId
        };
    }
}