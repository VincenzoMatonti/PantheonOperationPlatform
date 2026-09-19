namespace Hermes.Api.Endpoints.Requests;

public class GetEndpointByIdRequest
{
    public Guid EndpointId { get; set; }
}

public class GetEndpointByCodeRequest
{
    public string Code { get; set; } = string.Empty;
}

public class GetEndpointByTypeRequest
{
    public string Type { get; set; } = string.Empty;
}

public class GetEndpointCodeByIdRequest
{
    public Guid EndpointId { get; set; }
}