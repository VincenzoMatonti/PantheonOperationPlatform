namespace Hermes.Api.Clients.Requests;

public class GetClientOperationByIdRequest
{
    public Guid ClientOperationId { get; set; }
}

public class GetClientOperationRequest
{
    public Guid ClientId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class GetClientOperationsByClientIdRequest
{
    public Guid ClientId { get; set; }
}

public class GetClientOperationsByOperationTypeIdRequest
{
    public Guid OperationTypeId { get; set; }
}