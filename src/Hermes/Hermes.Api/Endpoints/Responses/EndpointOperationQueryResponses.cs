namespace Hermes.Api.Endpoints.Responses;

public class EndpointOperationResponse
{
    public Guid Id { get; set; }
    public Guid EndpointId { get; set; }
    public Guid OperationTypeId { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsDeleted { get; set; }
}
