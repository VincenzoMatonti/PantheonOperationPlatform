namespace Hermes.Api.Routes.Requests;

public class GetRouteByIdRequest
{
    public Guid RouteId { get; set; }
}

public class GetRouteRequest
{
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
}

public class GetRoutesByClientIdRequest
{
    public Guid ClientId { get; set; }
}

public class GetRoutesByOperationTypeIdRequest
{
    public Guid OperationTypeId { get; set; }
}

public class GetRoutesByEndpointIdRequest
{
    public Guid EndpointId { get; set; }
}

public class GetRoutesByClientIdAndOperationTypeIdRequest
{
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class GetRoutesByClientIdAndEndpointIdRequest
{
    public Guid ClientId { get; set; }
    public Guid EndpointId { get; set; }
}

public class GetRoutesByOperationTypeIdAndEndpointIdRequest
{
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
}

public class GetActiveRoutesByClientIdRequest
{
    public Guid ClientId { get; set; }
}

public class GetNonActiveRoutesByClientIdRequest
{
    public Guid ClientId { get; set; }
}

public class GetDeletedRoutesByClientIdRequest
{
    public Guid ClientId { get; set; }
}

public class GetActiveRoutesByOperationTypeIdRequest
{
    public Guid OperationTypeId { get; set; }
}

public class GetNonActiveRoutesByOperationTypeIdRequest
{
    public Guid OperationTypeId { get; set; }
}

public class GetDeletedRoutesByOperationTypeIdRequest
{
    public Guid OperationTypeId { get; set; }
}

public class GetActiveRoutesByEndpointIdRequest
{
    public Guid EndpointId { get; set; }
}

public class GetNonActiveRoutesByEndpointIdRequest
{
    public Guid EndpointId { get; set; }
}

public class GetDeletedRoutesByEndpointIdRequest
{
    public Guid EndpointId { get; set; }
}
