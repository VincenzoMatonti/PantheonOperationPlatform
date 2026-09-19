namespace Hermes.Api.Endpoints.Responses;

public class EndpointResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class EndpointCodeResponse
{
    public string Code { get; set; } = string.Empty;
}