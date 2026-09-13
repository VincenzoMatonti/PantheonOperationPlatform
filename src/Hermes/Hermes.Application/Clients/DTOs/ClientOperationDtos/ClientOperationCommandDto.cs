namespace Hermes.Application.Clients.DTOs.ClientOperationDtos;

public class CreateClientOperationCommand
{
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class EnableClientOperationCommand
{
    public Guid ClientOperationId { get; set; }
}

public class DisableClientOperationCommand
{
    public Guid ClientOperationId { get; set; }
}

public class DeleteClientOperationCommand
{
    public Guid ClientOperationId { get; set; }
}

public class RestoreClientOperationCommand
{
    public Guid ClientOperationId { get; set; }
}