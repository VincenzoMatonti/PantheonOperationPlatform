using Hermes.Application.Routes.DTOs;
using Hermes.Domain.Routes.Entities;
using Hermes.Domain.Routes.Repositories;

namespace Hermes.Application.Routes.Handlers;

public class RouteCommandHandler(IRouteRepository routeRepository)
{
    private readonly IRouteRepository _routeRepository = routeRepository;

    public async Task<Route> CreateRouteAsync(CreateRouteCommand command, CancellationToken cancellationToken = default)
    {
        var route = Route.Create(command.ClientId, command.OperationTypeId, command.EndpointId);
        await _routeRepository.AddAsync(route, cancellationToken);
        return route;
    }

    public async Task ActivateRouteAsync(Route route)
    {
        route.Activate();
        _routeRepository.Update(route);
        await Task.CompletedTask;
    }

    public async Task DeactivateRouteAsync(Route route)
    {
        route.Deactivate();
        _routeRepository.Update(route);
        await Task.CompletedTask;
    }

    public async Task DeleteRouteAsync(Route route)
    {
        route.Delete();
        _routeRepository.Update(route);
        await Task.CompletedTask;
    }

    public async Task RestoreRouteAsync(Route route)
    {
        route.Restore();
        _routeRepository.Update(route);
        await Task.CompletedTask;
    }

    public static CreateRouteDto ConvertRouteEntitiesToCreateDto(Route route)
    {
        return new CreateRouteDto
        {
            Id = route.Id,
            ClientId = route.ClientId,
            OperationTypeId = route.OperationTypeId,
            EndpointId = route.EndpointId,
            CreatedAt = route.CreatedAt
        };
    }

    public static List<CreateRouteDto> ConvertRouteEntitiesToCreateDto(List<Route> routes)
    {
        return [.. routes.Select(ConvertRouteEntitiesToCreateDto)];
    }

    public static ActivateRouteDto ConvertRouteEntitiesToActivateDto(Route route)
    {
        return new ActivateRouteDto
        {
            Id = route.Id,
            ClientId = route.ClientId,
            OperationTypeId = route.OperationTypeId,
            EndpointId = route.EndpointId,
            IsActive = route.IsActive,
            UpdatedAt = route.UpdatedAt
        };
    }

    public static List<ActivateRouteDto> ConvertRouteEntitiesToActivateDto(List<Route> routes)
    {
        return [.. routes.Select(ConvertRouteEntitiesToActivateDto)];
    }

    public static DeactivateRouteDto ConvertRouteEntitiesToDeactivateDto(Route route)
    {
        return new DeactivateRouteDto
        {
            Id = route.Id,
            ClientId = route.ClientId,
            OperationTypeId = route.OperationTypeId,
            EndpointId = route.EndpointId,
            IsActive = route.IsActive,
            UpdatedAt = route.UpdatedAt
        };
    }

    public static List<DeactivateRouteDto> ConvertRouteEntitiesToDeactivateDto(List<Route> routes)
    {
        return [.. routes.Select(ConvertRouteEntitiesToDeactivateDto)];
    }

    public static DeleteRouteDto ConvertRouteEntitiesToDeleteDto(Route route)
    {
        return new DeleteRouteDto
        {
            Id = route.Id,
            ClientId = route.ClientId,
            OperationTypeId = route.OperationTypeId,
            EndpointId = route.EndpointId,
            IsDeleted = route.IsDeleted,
            UpdatedAt = route.UpdatedAt
        };
    }

    public static List<DeleteRouteDto> ConvertRouteEntitiesToDeleteDto(List<Route> routes)
    {
        return [.. routes.Select(ConvertRouteEntitiesToDeleteDto)];
    }

    public static RestoreRouteDto ConvertRouteEntitiesToRestoreDto(Route route)
    {
        return new RestoreRouteDto
        {
            Id = route.Id,
            ClientId = route.ClientId,
            OperationTypeId = route.OperationTypeId,
            EndpointId = route.EndpointId,
            IsDeleted = route.IsDeleted,
            UpdatedAt = route.UpdatedAt
        };
    }

    public static List<RestoreRouteDto> ConvertRouteEntitiesToRestoreDto(List<Route> routes)
    {
        return [.. routes.Select(ConvertRouteEntitiesToRestoreDto)];
    }
}


