namespace Hermes.Api.Clients.Responses;

public class CreateClientOperationResponse
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class EnableClientOperationResponse
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DisableClientOperationResponse
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteClientOperationResponse
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreClientOperationResponse
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}