using Hermes.Api.Endpoints.Responses;
using Hermes.Application.Endpoints.DTOs.EndpointDtos;

namespace Hermes.Api.Endpoints.Mappings.Queries;

public class EndpointQueryResponseMapper
{
    public EndpointResponse ToResponse(EndpointDto dto)
    {
        return new EndpointResponse
        {
            Id = dto.Id,
            Code = dto.Code,
            Type = dto.Type,
        };
    }

    public List<EndpointResponse> ToResponse(List<EndpointDto> dtos)
    {
        return [.. dtos.Select(ToResponse)];
    }

    public EndpointCodeResponse ToResponse(EndpointCodeDto dto)
    {
        return new EndpointCodeResponse { Code = dto.Code };
    }

    public List<EndpointCodeResponse> ToResponse(List<EndpointCodeDto> dtos)
    {
        return [.. dtos.Select(ToResponse)];
    }
}