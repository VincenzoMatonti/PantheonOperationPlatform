using Hermes.Api.Routes.Requests;
using Hermes.Application.Routes.DTOs;

namespace Hermes.Api.Routes.Mappings.Queries;

public class RouteQueryRequestMapper
{
    public GetRouteByIdQuery ToQuery(GetRouteByIdRequest request)
    {
        return new GetRouteByIdQuery
        {
            RouteId = request.RouteId
        };
    }

    public GetRouteQuery ToQuery(GetRouteRequest request)
    {
        return new GetRouteQuery
        {
            ClientId = request.ClientId,
            OperationTypeId = request.OperationTypeId,
            EndpointId = request.EndpointId
        };
    }

    public GetRoutesByClientIdQuery ToQuery(GetRoutesByClientIdRequest request)
    {
        return new GetRoutesByClientIdQuery
        {
            ClientId = request.ClientId
        };
    }

    public GetRoutesByOperationTypeIdQuery ToQuery(GetRoutesByOperationTypeIdRequest request)
    {
        return new GetRoutesByOperationTypeIdQuery
        {
            OperationTypeId = request.OperationTypeId
        };
    }

    public GetRoutesByEndpointIdQuery ToQuery(GetRoutesByEndpointIdRequest request)
    {
        return new GetRoutesByEndpointIdQuery
        {
            EndpointId = request.EndpointId
        };
    }

    public GetRoutesByClientIdAndOperationTypeIdQuery ToQuery(
        GetRoutesByClientIdAndOperationTypeIdRequest request)
    {
        return new GetRoutesByClientIdAndOperationTypeIdQuery
        {
            ClientId = request.ClientId,
            OperationTypeId = request.OperationTypeId
        };
    }

    public GetRoutesByClientIdAndEndpointIdQuery ToQuery(
        GetRoutesByClientIdAndEndpointIdRequest request)
    {
        return new GetRoutesByClientIdAndEndpointIdQuery
        {
            ClientId = request.ClientId,
            EndpointId = request.EndpointId
        };
    }

    public GetRoutesByOperationTypeIdAndEndpointIdQuery ToQuery(
        GetRoutesByOperationTypeIdAndEndpointIdRequest request)
    {
        return new GetRoutesByOperationTypeIdAndEndpointIdQuery
        {
            OperationTypeId = request.OperationTypeId,
            EndpointId = request.EndpointId
        };
    }

    public GetActiveRoutesByClientIdQuery ToQuery(
        GetActiveRoutesByClientIdRequest request)
    {
        return new GetActiveRoutesByClientIdQuery
        {
            ClientId = request.ClientId
        };
    }

    public GetNonActiveRoutesByClientIdQuery ToQuery(
        GetNonActiveRoutesByClientIdRequest request)
    {
        return new GetNonActiveRoutesByClientIdQuery
        {
            ClientId = request.ClientId
        };
    }

    public GetDeletedRoutesByClientIdQuery ToQuery(
        GetDeletedRoutesByClientIdRequest request)
    {
        return new GetDeletedRoutesByClientIdQuery
        {
            ClientId = request.ClientId
        };
    }

    public GetActiveRoutesByOperationTypeIdQuery ToQuery(
        GetActiveRoutesByOperationTypeIdRequest request)
    {
        return new GetActiveRoutesByOperationTypeIdQuery
        {
            OperationTypeId = request.OperationTypeId
        };
    }

    public GetNonActiveRoutesByOperationTypeIdQuery ToQuery(
        GetNonActiveRoutesByOperationTypeIdRequest request)
    {
        return new GetNonActiveRoutesByOperationTypeIdQuery
        {
            OperationTypeId = request.OperationTypeId
        };
    }

    public GetDeletedRoutesByOperationTypeIdQuery ToQuery(
        GetDeletedRoutesByOperationTypeIdRequest request)
    {
        return new GetDeletedRoutesByOperationTypeIdQuery
        {
            OperationTypeId = request.OperationTypeId
        };
    }

    public GetActiveRoutesByEndpointIdQuery ToQuery(
        GetActiveRoutesByEndpointIdRequest request)
    {
        return new GetActiveRoutesByEndpointIdQuery
        {
            EndpointId = request.EndpointId
        };
    }

    public GetNonActiveRoutesByEndpointIdQuery ToQuery(
        GetNonActiveRoutesByEndpointIdRequest request)
    {
        return new GetNonActiveRoutesByEndpointIdQuery
        {
            EndpointId = request.EndpointId
        };
    }

    public GetDeletedRoutesByEndpointIdQuery ToQuery(
        GetDeletedRoutesByEndpointIdRequest request)
    {
        return new GetDeletedRoutesByEndpointIdQuery
        {
            EndpointId = request.EndpointId
        };
    }
}
