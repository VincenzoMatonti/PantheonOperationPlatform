namespace Hermes.Application.Clients.DTOs.ClientOperationDtos;

public class GetClientOperationByIdQuery
{
    public Guid ClientOperationId { get; set; }
}

public class GetClientOperationQuery
{
    public Guid ClientId { get; set; }

    public Guid OperationTypeId { get; set; }
}

public class GetClientOperationsByClientIdQuery
{
    public Guid ClientId { get; set; }
}

public class GetClientOperationsByOperationTypeIdQuery
{
    public Guid OperationTypeId { get; set; }
}