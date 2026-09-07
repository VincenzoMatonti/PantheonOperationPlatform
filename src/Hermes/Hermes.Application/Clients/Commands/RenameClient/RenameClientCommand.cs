namespace Hermes.Application.Clients.Commands.RenameClient;

public class RenameClientCommand
{
    public Guid ClientId { get; set; }
    public string Name { get; set; } = null!;
}