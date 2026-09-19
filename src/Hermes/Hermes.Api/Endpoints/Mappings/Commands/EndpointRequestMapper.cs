using Hermes.Api.Endpoints.Requests;
using Hermes.Application.Endpoints.Commands;

namespace Hermes.Api.Endpoints.Mappings.Commands;

public class EndpointRequestCommandMapping
{
    public CreateEndpointCommand ToCommand(CreateEndpointRequest request)
    {
        return new CreateEndpointCommand
        {
            Code = request.Code,
            Type = request.Type
        };
    }

    public ChangeEndpointTypeCommand ToCommand(ChangeEndpointTypeRequest request)
    {
        return new ChangeEndpointTypeCommand
        {
            EndpointId = request.EndpointId,
            NewType = request.NewType
        };
    }

    public RenameCodeEndpointCommand ToCommand(RenameCodeEndpointRequest request)
    {
        return new RenameCodeEndpointCommand
        {
            EndpointId = request.EndpointId,
            NewCode = request.NewCode
        };
    }

    public ActivateEndpointCommand ToCommand(ActivateEndpointRequest request)
    {
        return new ActivateEndpointCommand
        {
            EndpointId = request.EndpointId
        };
    }

    public DeactivateEndpointCommand ToCommand(DeactivateEndpointRequest request)
    {
        return new DeactivateEndpointCommand
        {
            EndpointId = request.EndpointId
        };
    }

    public DeleteEndpointCommand ToCommand(DeleteEndpointRequest request)
    {
        return new DeleteEndpointCommand
        {
            EndpointId = request.EndpointId
        };
    }

    public RestoreEndpointCommand ToCommand(RestoreEndpointRequest request)
    {
        return new RestoreEndpointCommand
        {
            EndpointId = request.EndpointId
        };
    }
}
