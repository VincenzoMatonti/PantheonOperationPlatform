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

public class CreateClientDto
{
    public Guid Id {get; set;}
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
}

public class RenameClientDto
{
    public Guid Id {get; set;}
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RenameClientCodeDto
{
    public Guid Id {get; set;}
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset UpdatedAt { get; set; }
}

public class ActivateClientDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeactivateClientDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteClientDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreClientDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}