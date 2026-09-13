using Hermes.Api.Clients.Responses;
using Hermes.Application.Clients.DTOs.ClientOperationDtos;

namespace Hermes.Api.Clients.Mappings.Queries;

public class ClientOperationQueryResponseMapper
{
    public ClientOperationResponse ToResponse(ClientOperationDto dto)
    {
        return new ClientOperationResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId
        };
    }

    public List<ClientOperationResponse> ToResponse(List<ClientOperationDto> dtos)
    {
        return [.. dtos.Select(ToResponse)];
    }
}