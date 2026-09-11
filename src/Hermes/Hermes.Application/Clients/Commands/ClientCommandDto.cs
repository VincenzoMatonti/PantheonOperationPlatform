namespace Hermes.Application.Clients.Commands;

public class ActivateClientCommand
{
    public Guid ClientId { get; set; }
}

public class DeactivateClientCommand
{
    public Guid ClientId { get; set; }
}

public class CreateClientCommand
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class RenameClientCommand
{
    public Guid ClientId { get; set; }
    public string Name { get; set; } = null!;
}