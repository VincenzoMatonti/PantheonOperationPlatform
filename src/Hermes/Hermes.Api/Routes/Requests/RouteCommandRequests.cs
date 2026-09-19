namespace Hermes.Api.Routes.Requests;

public class CreateRouteRequest
{
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
}

public class ActivateRouteRequest
{
    public Guid RouteId { get; set; }
}

public class DeactivateRouteRequest
{
    public Guid RouteId { get; set; }
}

public class DeleteRouteRequest
{
    public Guid RouteId { get; set; }
}

public class RestoreRouteRequest
{
    public Guid RouteId { get; set; }
}
