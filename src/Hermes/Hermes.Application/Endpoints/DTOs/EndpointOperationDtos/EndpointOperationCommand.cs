namespace Hermes.Application.Endpoints.DTOs.EndpointOperationDtos;

public class CreateEndpointOperationCommand
{
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
}

public class EnableEndpointOperationCommand
{
    public Guid EndpointOperationId { get; set; }
}

public class DisableEndpointOperationCommand
{
    public Guid EndpointOperationId { get; set; }
}

public class DeleteEndpointOperationCommand
{
    public Guid EndpointOperationId { get; set; }
}

public class RestoreEndpointOperationCommand
{
    public Guid EndpointOperationId { get; set; }
}

