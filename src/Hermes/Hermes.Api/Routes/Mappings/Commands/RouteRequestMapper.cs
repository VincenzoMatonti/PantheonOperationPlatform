using Hermes.Api.Routes.Requests;
using Hermes.Application.Routes.DTOs;

namespace Hermes.Api.Routes.Mappings.Commands;

public class RouteCommandRequestMapper
{
    public CreateRouteCommand ToCommand(CreateRouteRequest request)
    {
        return new CreateRouteCommand
        {
            ClientId = request.ClientId,
            OperationTypeId = request.OperationTypeId,
            EndpointId = request.EndpointId
        };
    }

    public ActivateRouteCommand ToCommand(ActivateRouteRequest request)
    {
        return new ActivateRouteCommand
        {
            RouteId = request.RouteId
        };
    }

    public DeactivateRouteCommand ToCommand(DeactivateRouteRequest request)
    {
        return new DeactivateRouteCommand
        {
            RouteId = request.RouteId
        };
    }

    public DeleteRouteCommand ToCommand(DeleteRouteRequest request)
    {
        return new DeleteRouteCommand
        {
            RouteId = request.RouteId
        };
    }

    public RestoreRouteCommand ToCommand(RestoreRouteRequest request)
    {
        return new RestoreRouteCommand
        {
            RouteId = request.RouteId
        };
    }
}
