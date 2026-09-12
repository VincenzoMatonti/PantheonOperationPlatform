namespace Hermes.Api.Clients.Requests;

public class GetClientByIdRequest
{
    public Guid ClientId { get; set; }
}

public class GetClientByCodeRequest
{
    public string Code { get; set; } = null!;
}

public class GetClientCodeByIdRequest
{
    public Guid ClientId { get; set; }
}

