namespace Hermes.Application.Routes.DTOs;

public class RouteDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
}

public class CreateRouteDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class ActivateRouteDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeactivateRouteDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteRouteDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreRouteDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public Guid EndpointId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

