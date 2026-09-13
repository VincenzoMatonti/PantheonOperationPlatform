namespace Hermes.Application.Clients.DTOs.ClientOperationDtos;

public class ClientOperationDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class CreateClientOperationDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class EnableClientOperationDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DisableClientOperationDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteClientOperationDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreClientOperationDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}