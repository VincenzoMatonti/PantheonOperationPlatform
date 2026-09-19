namespace Hermes.Application.Routes.DTOs;

public class CreateRouteCommand
{
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
}

public class ActivateRouteCommand
{
    public Guid RouteId { get; set; }
}

public class DeactivateRouteCommand
{
    public Guid RouteId { get; set; }
}

public class DeleteRouteCommand
{
    public Guid RouteId { get; set; }
}

public class RestoreRouteCommand
{
    public Guid RouteId { get; set; }
}
