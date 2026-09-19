using Hermes.Application.Routes.DTOs;
using Hermes.Application.Routes.Exceptions;
using Hermes.Application.Routes.Handlers;

namespace Hermes.Application.Routes.UseCases;

public class RouteUseCaseHandler(RouteQueryHandler routeQueryHandler, RouteCommandHandler routeCommandHandler)
{
    private readonly RouteQueryHandler _routeQueryHandler = routeQueryHandler;
    private readonly RouteCommandHandler _routeCommandHandler = routeCommandHandler;

    // USE CASE COMMAND
    public async Task<CreateRouteDto> CreateRouteAsync(CreateRouteCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetRouteQuery { ClientId = command.ClientId, OperationTypeId = command.OperationTypeId, EndpointId = command.EndpointId };
        var existingRoute = await _routeQueryHandler.GetRouteAsync(query, cancellationToken);
        if (existingRoute != null) throw new RouteAlreadyExistsException(command.ClientId, command.OperationTypeId, command.EndpointId);
        var route = await _routeCommandHandler.CreateRouteAsync(command, cancellationToken);
        return RouteCommandHandler.ConvertRouteEntitiesToCreateDto(route);
    }

    public async Task<ActivateRouteDto> ActivateRouteAsync(ActivateRouteCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetRouteByIdQuery { RouteId = command.RouteId };
        var route = await _routeQueryHandler.GetRouteByIdAsync(query, cancellationToken);
        if (route != null)
        {
            await _routeCommandHandler.ActivateRouteAsync(route);
            return RouteCommandHandler.ConvertRouteEntitiesToActivateDto(route);
        }
        throw new RouteNotFoundException(command.RouteId);
    }

    public async Task<DeactivateRouteDto> DeactivateRouteAsync(DeactivateRouteCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetRouteByIdQuery { RouteId = command.RouteId };
        var route = await _routeQueryHandler.GetRouteByIdAsync(query, cancellationToken);
        if (route != null)
        {
            await _routeCommandHandler.DeactivateRouteAsync(route);
            return RouteCommandHandler.ConvertRouteEntitiesToDeactivateDto(route);
        }
        throw new RouteNotFoundException(command.RouteId);
    }

    public async Task<DeleteRouteDto> DeleteRouteAsync(DeleteRouteCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetRouteByIdQuery { RouteId = command.RouteId };
        var route = await _routeQueryHandler.GetRouteByIdAsync(query, cancellationToken);
        if (route != null)
        {
            await _routeCommandHandler.DeleteRouteAsync(route);
            return RouteCommandHandler.ConvertRouteEntitiesToDeleteDto(route);
        }
        throw new RouteNotFoundException(command.RouteId);
    }

    public async Task<RestoreRouteDto> RestoreRouteAsync(RestoreRouteCommand command, CancellationToken cancellationToken = default)
    {
        var query = new GetRouteByIdQuery { RouteId = command.RouteId };
        var route = await _routeQueryHandler.GetRouteByIdAsync(query, cancellationToken);
        if (route != null)
        {
            await _routeCommandHandler.RestoreRouteAsync(route);
            return RouteCommandHandler.ConvertRouteEntitiesToRestoreDto(route);
        }
        throw new RouteNotFoundException(command.RouteId);
    }

    // USE CASE QUERY
    public async Task<RouteDto> GetRouteByIdAsync(GetRouteByIdQuery query, CancellationToken cancellationToken = default)
    {
        var route = await _routeQueryHandler.GetRouteByIdAsync(query, cancellationToken);
        if (route != null) return RouteQueryHandler.ConvertRouteEntitiesToDto(route);
        throw new RouteNotFoundException(query.RouteId);
    }

    public async Task<RouteDto> GetRouteAsync(GetRouteQuery query, CancellationToken cancellationToken = default)
    {
        var route = await _routeQueryHandler.GetRouteAsync(query, cancellationToken);
        if (route != null) return RouteQueryHandler.ConvertRouteEntitiesToDto(route);
        throw new RouteNotFoundByClientOperationTypeAndEndpointException(query.ClientId, query.OperationTypeId, query.EndpointId);
    }

    public async Task<List<RouteDto>> GetRoutesByClientIdAsync(GetRoutesByClientIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetRoutesByClientIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetRoutesByOperationTypeIdAsync(GetRoutesByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetRoutesByOperationTypeIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetRoutesByEndpointIdAsync(GetRoutesByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetRoutesByEndpointIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetRoutesByClientIdAndOperationTypeIdAsync(GetRoutesByClientIdAndOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetRoutesByClientIdAndOperationTypeIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetRoutesByClientIdAndEndpointIdAsync(GetRoutesByClientIdAndEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetRoutesByClientIdAndEndpointIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetRoutesByOperationTypeIdAndEndpointIdAsync(GetRoutesByOperationTypeIdAndEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetRoutesByOperationTypeIdAndEndpointIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetAllAsync(cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetActiveRoutesAsync(CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetActiveRoutesAsync(cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetNonActiveRoutesAsync(CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetNonActiveRoutesAsync(cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetDeletedRoutesAsync(CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetDeletedRoutesAsync(cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetActiveRoutesByClientIdAsync(GetActiveRoutesByClientIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetActiveRoutesByClientIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetNonActiveRoutesByClientIdAsync(GetNonActiveRoutesByClientIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetNonActiveRoutesByClientIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetDeletedRoutesByClientIdAsync(GetDeletedRoutesByClientIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetDeletedRoutesByClientIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetActiveRoutesByOperationTypeIdAsync(GetActiveRoutesByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetActiveRoutesByOperationTypeIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetNonActiveRoutesByOperationTypeIdAsync(GetNonActiveRoutesByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetNonActiveRoutesByOperationTypeIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetDeletedRoutesByOperationTypeIdAsync(GetDeletedRoutesByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetDeletedRoutesByOperationTypeIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetActiveRoutesByEndpointIdAsync(GetActiveRoutesByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetActiveRoutesByEndpointIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetNonActiveRoutesByEndpointIdAsync(GetNonActiveRoutesByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetNonActiveRoutesByEndpointIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }

    public async Task<List<RouteDto>> GetDeletedRoutesByEndpointIdAsync(GetDeletedRoutesByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        var routes = await _routeQueryHandler.GetDeletedRoutesByEndpointIdAsync(query, cancellationToken);
        if (routes == null || routes.Count == 0) return [];
        return RouteQueryHandler.ConvertRouteEntitiesToDto(routes);
    }
}

