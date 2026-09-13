namespace Hermes.Api.Clients.Requests;

public class CreateClientOperationRequest
{
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class EnableClientOperationRequest
{
    public Guid ClientOperationId { get; set; }
}

public class DisableClientOperationRequest
{
    public Guid ClientOperationId { get; set; }
}

public class DeleteClientOperationRequest
{
    public Guid ClientOperationId { get; set; }
}

public class RestoreClientOperationRequest
{
    public Guid ClientOperationId { get; set; }
}