namespace Hermes.Api.Clients.Responses;

public class CreateClientResponse
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}