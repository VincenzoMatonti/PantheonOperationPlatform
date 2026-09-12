namespace Hermes.Api.Clients.Requests;

public class CreateClientRequest
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;
}

public class RenameClientRequest
{
    public Guid ClientId { get; set; }

    public string Name { get; set; } = null!;
}

public class RenameClientCodeRequest
{
    public Guid ClientId { get; set; }

    public string NewCode { get; set; } = null!;
}

public class ActivateClientRequest
{
    public Guid ClientId { get; set; }
}

public class DeactivateClientRequest
{
    public Guid ClientId { get; set; }
}

public class DeleteClientRequest
{
    public Guid ClientId { get; set; }
}

public class RestoreClientRequest
{
    public Guid ClientId { get; set; }
}