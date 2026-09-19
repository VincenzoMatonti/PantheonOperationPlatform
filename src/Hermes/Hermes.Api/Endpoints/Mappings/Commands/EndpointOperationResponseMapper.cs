using Hermes.Api.Endpoints.Responses;
using Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;

namespace Hermes.Api.Endpoints.Mappings.Commands;

public class EndpointOperationResponseCommandMapping
{
    public CreateEndpointOperationResponse ToResponse(CreateEndpointOperationDto dto)
    {
        return new CreateEndpointOperationResponse
        {
            Id = dto.Id,
            EndpointId = dto.EndpointId,
            OperationTypeId = dto.OperationTypeId,
            CreatedAt = dto.CreatedAt
        };
    }

    public EnableEndpointOperationResponse ToResponse(EnableEndpointOperationDto dto)
    {
        return new EnableEndpointOperationResponse
        {
            Id = dto.Id,
            EndpointId = dto.EndpointId,
            OperationTypeId = dto.OperationTypeId,
            IsEnabled = dto.IsEnabled,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public DisableEndpointOperationResponse ToResponse(DisableEndpointOperationDto dto)
    {
        return new DisableEndpointOperationResponse
        {
            Id = dto.Id,
            EndpointId = dto.EndpointId,
            OperationTypeId = dto.OperationTypeId,
            IsEnabled = dto.IsEnabled,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public DeleteEndpointOperationResponse ToResponse(DeleteEndpointOperationDto dto)
    {
        return new DeleteEndpointOperationResponse
        {
            Id = dto.Id,
            EndpointId = dto.EndpointId,
            OperationTypeId = dto.OperationTypeId,
            IsDeleted = dto.IsDeleted,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public RestoreEndpointOperationResponse ToResponse(RestoreEndpointOperationDto dto)
    {
        return new RestoreEndpointOperationResponse
        {
            Id = dto.Id,
            EndpointId = dto.EndpointId,
            OperationTypeId = dto.OperationTypeId,
            IsDeleted = dto.IsDeleted,
            UpdatedAt = dto.UpdatedAt
        };
    }
}