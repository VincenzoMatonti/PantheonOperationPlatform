using Hermes.Api.Clients.Requests;
using Hermes.Application.Clients.DTOs.ClientDTOs;

namespace Hermes.Api.Clients.Mappings.Commands;

public class ClientCommandRequestMapper
{
    public CreateClientCommand ToCommand(CreateClientRequest request)
    {
        return new CreateClientCommand
        {
            Code = request.Code,
            Name = request.Name
        };
    }

    public RenameClientCommand ToCommand(RenameClientRequest request)
    {
        return new RenameClientCommand
        {
            ClientId = request.ClientId,
            NewName = request.Name
        };
    }

    public RenameCodeClientCommand ToCommand(RenameClientCodeRequest request)
    {
        return new RenameCodeClientCommand
        {
            ClientId = request.ClientId,
            NewCode = request.NewCode
        };
    }

    public ActivateClientCommand ToCommand(ActivateClientRequest request)
    {
        return new ActivateClientCommand
        {
            ClientId = request.ClientId
        };
    }

    public DeactivateClientCommand ToCommand(DeactivateClientRequest request)
    {
        return new DeactivateClientCommand
        {
            ClientId = request.ClientId
        };
    }

    public DeletedClientCommand ToCommand(DeleteClientRequest request)
    {
        return new DeletedClientCommand
        {
            ClientId = request.ClientId
        };
    }

    public RestoreClientCommand ToCommand(RestoreClientRequest request)
    {
        return new RestoreClientCommand
        {
            ClientId = request.ClientId
        };
    }  
}