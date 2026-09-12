namespace Hermes.Application.Clients.DTOs.ClientDTOs;

public class GetClientByCodeQuery
{
    public string Code { get; set; } = null!;
}

public class GetClientByIdQuery
{
    public Guid ClientId { get; set; }
}

public class GetClientCodeByIdQuery
{
    public Guid ClientId { get; set; }
}

