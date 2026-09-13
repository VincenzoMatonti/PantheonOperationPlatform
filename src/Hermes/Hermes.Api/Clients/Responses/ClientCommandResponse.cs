namespace Hermes.Api.Clients.Responses;

public class CreateClientResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}

public class RenameClientResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RenameClientCodeResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset UpdatedAt { get; set; }
}

public class ActivateClientResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeactivateClientResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteClientResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreClientResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}