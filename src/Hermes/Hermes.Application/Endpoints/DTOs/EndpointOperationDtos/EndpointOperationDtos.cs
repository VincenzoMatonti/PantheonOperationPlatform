namespace Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;

public class EndpointOperationDto
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsDeleted { get; set; }
}

public class CreateEndpointOperationDto
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class EnableEndpointOperationDto
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DisableEndpointOperationDto
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteEndpointOperationDto
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreEndpointOperationDto
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
