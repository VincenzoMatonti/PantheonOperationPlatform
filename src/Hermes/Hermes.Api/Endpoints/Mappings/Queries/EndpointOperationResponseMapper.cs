using Hermes.Api.Endpoints.Responses;
using Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;

namespace Hermes.Api.Endpoints.Mappings.Queries;

public class EndpointOperationQueryResponseMapping
{
    public EndpointOperationResponse ToResponse(EndpointOperationDto dto)
    {
        return new EndpointOperationResponse
        {
            Id = dto.Id,
            EndpointId = dto.EndpointId,
            OperationTypeId = dto.OperationTypeId,
            IsEnabled = dto.IsEnabled,
            IsDeleted = dto.IsDeleted
        };
    }

    public List<EndpointOperationResponse> ToResponse(List<EndpointOperationDto> dtos)
    {
        return dtos.Select(ToResponse).ToList();
    }
}
