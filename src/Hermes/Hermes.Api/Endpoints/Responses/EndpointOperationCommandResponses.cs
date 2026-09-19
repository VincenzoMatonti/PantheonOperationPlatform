namespace Hermes.Api.Endpoints.Responses;

public class CreateEndpointOperationResponse
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class EnableEndpointOperationResponse
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DisableEndpointOperationResponse
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteEndpointOperationResponse
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreEndpointOperationResponse
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
