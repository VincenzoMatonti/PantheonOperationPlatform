using Hermes.Api.Clients.Responses;
using Hermes.Application.Clients.DTOs.ClientDTOs;

namespace Hermes.Api.Clients.Mappings.Commands;

public class ClientCommandResponseMapper
{
    public CreateClientResponse ToResponse(ClientDto client)
    {
        return new CreateClientResponse
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name
        };
    }
}