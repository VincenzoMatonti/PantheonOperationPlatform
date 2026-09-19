namespace Hermes.Application.Endpoints.Queries;

public class GetEndpointByIdQuery
{
    public Guid EndpointId { get; set; }
}

public class GetEndpointByCodeQuery
{
    public string Code { get; set; } = null!;
}

public class GetEndpointByTypeQuery
{
    public string Type { get; set; } = null!;
}

public class GetEndpointCodeByIdQuery
{
    public Guid EndpointId { get; set; }
}