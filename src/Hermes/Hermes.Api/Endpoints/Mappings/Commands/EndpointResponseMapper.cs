using Hermes.Api.Endpoints.Responses;
using Hermes.Application.Endpoints.DTOs.EndpointDtos;

namespace Hermes.Api.Endpoints.Mappings.Commands;

public class EndpointResponseCommandMapping
{
    public CreateEndpointResponse ToResponse(CreateEndpointDto dto)
    {
        return new CreateEndpointResponse
        {
            Id = dto.Id,
            Code = dto.Code,
            Type = dto.Type,
            CreatedAt = dto.CreatedAt
        };
    }

    public ChangeEndpointTypeResponse ToResponse(ChangeEndpointTypeDto dto)
    {
        return new ChangeEndpointTypeResponse
        {
            Id = dto.Id,
            Code = dto.Code,
            Type = dto.Type,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public RenameCodeEndpointResponse ToResponse(RenameCodeEndpointDto dto)
    {
        return new RenameCodeEndpointResponse
        {
            Id = dto.Id,
            Code = dto.Code,
            Type = dto.Type,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public ActivateEndpointResponse ToResponse(ActivateEndpointDto dto)
    {
        return new ActivateEndpointResponse
        {
            Id = dto.Id,
            Code = dto.Code,
            Type = dto.Type,
            IsActive = dto.IsActive,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public DeactivateEndpointResponse ToResponse(DeactivateEndpointDto dto)
    {
        return new DeactivateEndpointResponse
        {
            Id = dto.Id,
            Code = dto.Code,
            Type = dto.Type,
            IsActive = dto.IsActive,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public DeleteEndpointResponse ToResponse(DeleteEndpointDto dto)
    {
        return new DeleteEndpointResponse
        {
            Id = dto.Id,
            Code = dto.Code,
            Type = dto.Type,
            IsDeleted = dto.IsDeleted,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public RestoreEndpointResponse ToResponse(RestoreEndpointDto dto)
    {
        return new RestoreEndpointResponse
        {
            Id = dto.Id,
            Code = dto.Code,
            Type = dto.Type,
            IsDeleted = dto.IsDeleted,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
