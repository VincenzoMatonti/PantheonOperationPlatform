namespace Hermes.Application.Clients.DTOs.ClientDTOs;

public class ClientDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class ClientCodeDto
{
    public string Code { get; set; } = null!;
}