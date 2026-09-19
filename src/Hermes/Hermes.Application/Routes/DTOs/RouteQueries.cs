namespace Hermes.Application.Routes.DTOs;

public class GetRouteByIdQuery 
{
    public Guid RouteId { get; set; }
}

public class GetRouteQuery 
{
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
}

public class GetRoutesByClientIdQuery
{
    public Guid ClientId { get; set; }
}

public class GetRoutesByOperationTypeIdQuery
{
    public Guid OperationTypeId { get; set; }
}

public class GetRoutesByEndpointIdQuery
{
    public Guid EndpointId { get; set; }
}

public class GetRoutesByClientIdAndOperationTypeIdQuery
{
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class GetRoutesByClientIdAndEndpointIdQuery
{
    public Guid ClientId { get; set; }
    public Guid EndpointId { get; set; }
}

public class GetRoutesByOperationTypeIdAndEndpointIdQuery
{
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
}

public class GetActiveRoutesByClientIdQuery
{
    public Guid ClientId { get; set; }
}

public class GetNonActiveRoutesByClientIdQuery
{
    public Guid ClientId { get; set; }
}

public class GetDeletedRoutesByClientIdQuery
{
    public Guid ClientId { get; set; }
}

public class GetActiveRoutesByOperationTypeIdQuery
{
    public Guid OperationTypeId { get; set; }
}

public class GetNonActiveRoutesByOperationTypeIdQuery
{
    public Guid OperationTypeId { get; set; }
}

public class GetDeletedRoutesByOperationTypeIdQuery
{
    public Guid OperationTypeId { get; set; }
}

public class GetActiveRoutesByEndpointIdQuery
{
    public Guid EndpointId { get; set; }
}

public class GetNonActiveRoutesByEndpointIdQuery
{
    public Guid EndpointId { get; set; }
}

public class GetDeletedRoutesByEndpointIdQuery
{
    public Guid EndpointId { get; set; }
}

