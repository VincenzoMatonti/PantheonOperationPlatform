namespace Hermes.Api.Clients.Responses;

public class ClientResponse
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}

public class ClientCodeResponse
{
    public string Code { get; set; } = null!;
}

