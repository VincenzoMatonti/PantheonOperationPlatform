using Hermes.Api.Routes.Responses;
using Hermes.Application.Routes.DTOs;

namespace Hermes.Api.Routes.Mappings.Queries;

public class RouteQueryResponseMapper
{
    public RouteResponse ToResponse(RouteDto dto)
    {
        return new RouteResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            EndpointId = dto.EndpointId
        };
    }

    public List<RouteResponse> ToResponse(List<RouteDto> dtos)
    {
        return [.. dtos.Select(ToResponse)];
    }
}
