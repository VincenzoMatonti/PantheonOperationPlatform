using Hermes.Api.Clients.Responses;
using Hermes.Application.Clients.DTOs.ClientOperationDtos;

namespace Hermes.Api.Clients.Mappings.Commands;

public class ClientOperationCommandResponseMapper
{
    public CreateClientOperationResponse ToResponse(CreateClientOperationDto dto)
    {
        return new CreateClientOperationResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            IsEnabled = dto.IsEnabled,
            IsDeleted = dto.IsDeleted,
            CreatedAt = dto.CreatedAt,
        };
    }

    public EnableClientOperationResponse ToResponse(EnableClientOperationDto dto)
    {
        return new EnableClientOperationResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            IsEnabled = dto.IsEnabled,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public DisableClientOperationResponse ToResponse(DisableClientOperationDto dto)
    {
        return new DisableClientOperationResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            IsEnabled = dto.IsEnabled,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public DeleteClientOperationResponse ToResponse(DeleteClientOperationDto dto)
    {
        return new DeleteClientOperationResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            IsDeleted = dto.IsDeleted,
            UpdatedAt = dto.UpdatedAt
        };
    }

    public RestoreClientOperationResponse ToResponse(RestoreClientOperationDto dto)
    {
        return new RestoreClientOperationResponse
        {
            Id = dto.Id,
            ClientId = dto.ClientId,
            OperationTypeId = dto.OperationTypeId,
            IsDeleted = dto.IsDeleted,
            UpdatedAt = dto.UpdatedAt
        };
    }
}