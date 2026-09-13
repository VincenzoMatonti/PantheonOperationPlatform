using Hermes.Api.Clients.Responses;
using Hermes.Application.Clients.DTOs.ClientDTOs;

namespace Hermes.Api.Clients.Mappings.Commands;

public class ClientCommandResponseMapper
{
    public CreateClientResponse ToResponse(CreateClientDto client)
    {
        return new CreateClientResponse
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name,
            CreatedAt = client.CreatedAt
        };
    }

    public RenameClientResponse ToResponse(RenameClientDto client)
    {
        return new RenameClientResponse
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name,
            UpdatedAt = client.UpdatedAt
        };
    }

    public RenameClientCodeResponse ToResponse(RenameClientCodeDto client)
    {
        return new RenameClientCodeResponse
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name,
            UpdatedAt = client.UpdatedAt
        };
    }

    public ActivateClientResponse ToResponse(ActivateClientDto client)
    {
        return new ActivateClientResponse
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name,
            IsActive = client.IsActive,
            UpdatedAt = client.UpdatedAt
        };
    }

    public DeactivateClientResponse ToResponse(DeactivateClientDto client)
    {
        return new DeactivateClientResponse
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name,
            IsActive = client.IsActive,
            UpdatedAt = client.UpdatedAt
        };
    }

    public DeleteClientResponse ToResponse(DeleteClientDto client)
    {
        return new DeleteClientResponse
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name,
            IsDeleted = client.IsDeleted,
            UpdatedAt = client.UpdatedAt
        };
    }

    public RestoreClientResponse ToResponse(RestoreClientDto client)
    {
        return new RestoreClientResponse
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name,
            IsDeleted = client.IsDeleted,
            UpdatedAt = client.UpdatedAt
        };
    }
}