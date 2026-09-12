using Hermes.Api.Clients.Responses;
using Hermes.Application.Clients.DTOs.ClientDTOs;

namespace Hermes.Api.Clients.Mappings.Queries;

public class ClientQueryResponseMapper
{
    public ClientResponse ToResponse(ClientDto client)
    {
        return new ClientResponse
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name
        };
    }

    public List<ClientResponse> ToResponse(List<ClientDto> clients)
    {
        return [.. clients.Select(ToResponse)];
    }

    public ClientCodeResponse ToResponse(ClientCodeDto clientCode)
    {
        return new ClientCodeResponse
        {
            Code = clientCode.Code
        };
    }

    public List<ClientCodeResponse> ToCodeResponse(List<ClientCodeDto> clientCodes)
    {
        return [.. clientCodes.Select(ToResponse)];
    }
}

