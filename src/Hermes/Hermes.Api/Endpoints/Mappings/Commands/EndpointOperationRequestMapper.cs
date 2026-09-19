using Hermes.Api.Endpoints.Requests;
using Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;

namespace Hermes.Api.Endpoints.Mappings.Commands;

public class EndpointOperationRequestCommandMapping
{
    public CreateEndpointOperationCommand ToCommand(CreateEndpointOperationRequest request)
    {
        return new CreateEndpointOperationCommand
        {
            EndpointId = request.EndpointId,
            OperationTypeId = request.OperationTypeId
        };
    }

    public EnableEndpointOperationCommand ToCommand(EnableEndpointOperationRequest request)
    {
        return new EnableEndpointOperationCommand
        {
            EndpointOperationId = request.EndpointOperationId
        };
    }

    public DisableEndpointOperationCommand ToCommand(DisableEndpointOperationRequest request)
    {
        return new DisableEndpointOperationCommand
        {
            EndpointOperationId = request.EndpointOperationId
        };
    }

    public DeleteEndpointOperationCommand ToCommand(DeleteEndpointOperationRequest request)
    {
        return new DeleteEndpointOperationCommand
        {
            EndpointOperationId = request.EndpointOperationId
        };
    }

    public RestoreEndpointOperationCommand ToCommand(RestoreEndpointOperationRequest request)
    {
        return new RestoreEndpointOperationCommand
        {
            EndpointOperationId = request.EndpointOperationId
        };
    }
}



