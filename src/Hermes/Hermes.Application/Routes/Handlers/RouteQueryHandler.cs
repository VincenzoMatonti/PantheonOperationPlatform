using Hermes.Application.Routes.DTOs;
using Hermes.Domain.Routes.Entities;
using Hermes.Domain.Routes.Repositories;

namespace Hermes.Application.Routes.Handlers;

public class RouteQueryHandler(IRouteRepository routeRepository)
{
    private readonly IRouteRepository _routeRepository = routeRepository;

    public static RouteDto ConvertRouteEntitiesToDto(Route route)
    {
        return new RouteDto
        {
            Id = route.Id,
            ClientId = route.ClientId,
            OperationTypeId = route.OperationTypeId,
            EndpointId = route.EndpointId
        };
    }

    public static List<RouteDto> ConvertRouteEntitiesToDto(List<Route> routes)
    {
        return [.. routes.Select(ConvertRouteEntitiesToDto)];
    }

    public async Task<Route?> GetRouteByIdAsync(GetRouteByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetByIdAsync(query.RouteId, cancellationToken);
    }

    public async Task<Route?> GetRouteAsync(GetRouteQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetAsync(query.ClientId, query.OperationTypeId, query.EndpointId, cancellationToken);
    }

    public async Task<List<Route>> GetRoutesByClientIdAsync(GetRoutesByClientIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetByClientIdAsync(query.ClientId, cancellationToken);
    }

    public async Task<List<Route>> GetRoutesByOperationTypeIdAsync(GetRoutesByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetByOperationTypeIdAsync(query.OperationTypeId, cancellationToken);
    }

    public async Task<List<Route>> GetRoutesByEndpointIdAsync(GetRoutesByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetByEndpointIdAsync(query.EndpointId, cancellationToken);
    }

    public async Task<List<Route>> GetRoutesByClientIdAndOperationTypeIdAsync(GetRoutesByClientIdAndOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetByClientIdAndOperationTypeIdAsync(query.ClientId, query.OperationTypeId, cancellationToken);
    }

    public async Task<List<Route>> GetRoutesByClientIdAndEndpointIdAsync(GetRoutesByClientIdAndEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetByClientIdAndEndpointIdAsync(query.ClientId, query.EndpointId, cancellationToken);
    }

    public async Task<List<Route>> GetRoutesByOperationTypeIdAndEndpointIdAsync(GetRoutesByOperationTypeIdAndEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetByOperationTypeIdAndEndpointIdAsync(query.OperationTypeId, query.EndpointId, cancellationToken);
    }

    public async Task<List<Route>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetAllAsync(cancellationToken);
    }

    public async Task<List<Route>> GetActiveRoutesAsync(CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetActiveAsync(cancellationToken);
    }

    public async Task<List<Route>> GetNonActiveRoutesAsync(CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetNonActiveAsync(cancellationToken);
    }

    public async Task<List<Route>> GetDeletedRoutesAsync(CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetDeletedAsync(cancellationToken);
    }

    public async Task<List<Route>> GetActiveRoutesByClientIdAsync(GetActiveRoutesByClientIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetActiveByClientIdAsync(query.ClientId, cancellationToken);
    }

    public async Task<List<Route>> GetNonActiveRoutesByClientIdAsync(GetNonActiveRoutesByClientIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetNonActiveByClientIdAsync(query.ClientId, cancellationToken);
    }

    public async Task<List<Route>> GetDeletedRoutesByClientIdAsync(GetDeletedRoutesByClientIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetDeletedByClientIdAsync(query.ClientId, cancellationToken);
    }

    public async Task<List<Route>> GetActiveRoutesByOperationTypeIdAsync(GetActiveRoutesByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetActiveByOperationTypeIdAsync(query.OperationTypeId, cancellationToken);
    }

    public async Task<List<Route>> GetNonActiveRoutesByOperationTypeIdAsync(GetNonActiveRoutesByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetNonActiveByOperationTypeIdAsync(query.OperationTypeId, cancellationToken);
    }

    public async Task<List<Route>> GetDeletedRoutesByOperationTypeIdAsync(GetDeletedRoutesByOperationTypeIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetDeletedByOperationTypeIdAsync(query.OperationTypeId, cancellationToken);
    }

    public async Task<List<Route>> GetActiveRoutesByEndpointIdAsync(GetActiveRoutesByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetActiveByEndpointIdAsync(query.EndpointId, cancellationToken);
    }

    public async Task<List<Route>> GetNonActiveRoutesByEndpointIdAsync(GetNonActiveRoutesByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetNonActiveByEndpointIdAsync(query.EndpointId, cancellationToken);
    }

    public async Task<List<Route>> GetDeletedRoutesByEndpointIdAsync(GetDeletedRoutesByEndpointIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _routeRepository.GetDeletedByEndpointIdAsync(query.EndpointId, cancellationToken);
    }
}


