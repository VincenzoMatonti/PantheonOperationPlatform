using Hermes.Api.Clients.Requests;
using Hermes.Application.Clients.DTOs.ClientOperationDtos;

namespace Hermes.Api.Clients.Mappings.Commands;

public class ClientOperationCommandRequestMapper
{
    public CreateClientOperationCommand ToCommand(CreateClientOperationRequest request)
    {
        return new CreateClientOperationCommand
        {
            ClientId = request.ClientId,
            OperationTypeId = request.OperationTypeId
        };
    }

    public EnableClientOperationCommand ToCommand(EnableClientOperationRequest request)
    {
        return new EnableClientOperationCommand
        {
            ClientOperationId = request.ClientOperationId
        };
    }

    public DisableClientOperationCommand ToCommand(DisableClientOperationRequest request)
    {
        return new DisableClientOperationCommand
        {
            ClientOperationId = request.ClientOperationId
        };
    }

    public DeleteClientOperationCommand ToCommand(DeleteClientOperationRequest request)
    {
        return new DeleteClientOperationCommand
        {
            ClientOperationId = request.ClientOperationId
        };
    }

    public RestoreClientOperationCommand ToCommand(RestoreClientOperationRequest request)
    {
        return new RestoreClientOperationCommand
        {
            ClientOperationId = request.ClientOperationId
        };
    }
}