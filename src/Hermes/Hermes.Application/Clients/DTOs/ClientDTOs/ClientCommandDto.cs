namespace Hermes.Application.Clients.DTOs.ClientDTOs;

public class ActivateClientCommand
{
    public Guid ClientId { get; set; }
}

public class DeactivateClientCommand
{
    public Guid ClientId { get; set; }
}

public class DeletedClientCommand
{
    public Guid ClientId { get; set; }
}

public class RestoreClientCommand
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
    public string NewName { get; set; } = null!;
}

public class RenameCodeClientCommand
{
    public Guid ClientId { get; set; }
    public string NewCode { get; set; } = null!;
}