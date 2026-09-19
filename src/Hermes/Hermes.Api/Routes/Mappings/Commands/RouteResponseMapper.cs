using Hermes.Api.Routes.Responses;
using Hermes.Application.Routes.DTOs;

namespace Hermes.Api.Routes.Mappings.Commands;

public class RouteCommandResponseMapper
{
    public CreateRouteResponse ToResponse(CreateRouteDto dto)
    {
        return new CreateRouteResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            EndpointId = dto.EndpointId,
            CreatedAt = dto.CreatedAt
        };
    }

    public ActivateRouteResponse ToResponse(ActivateRouteDto dto)
    {
        return new ActivateRouteResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            EndpointId = dto.EndpointId,
            IsActive = dto.IsActive,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public DeactivateRouteResponse ToResponse(DeactivateRouteDto dto)
    {
        return new DeactivateRouteResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            EndpointId = dto.EndpointId,
            IsActive = dto.IsActive,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public DeleteRouteResponse ToResponse(DeleteRouteDto dto)
    {
        return new DeleteRouteResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            EndpointId = dto.EndpointId,
            IsDeleted = dto.IsDeleted,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public RestoreRouteResponse ToResponse(RestoreRouteDto dto)
    {
        return new RestoreRouteResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            EndpointId = dto.EndpointId,
            IsDeleted = dto.IsDeleted,
            UpdatedAt = dto.UpdatedAt
        };
    }
}
