namespace Hermes.Application.Clients.Queries;

public class GetClientByCodeQuery
{
    public string Code { get; set; } = null!;
}

public class GetClientByIdQuery
{
    public Guid ClientId { get; set; }
}

