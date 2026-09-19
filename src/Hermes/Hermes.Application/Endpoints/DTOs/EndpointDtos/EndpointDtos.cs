namespace Hermes.Application.Endpoints.DTOs.EndpointDtos;

public class EndpointDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
}

public class RenameEndpointDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
}

public class RenameCodeEndpointDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
}

public class EndpointCodeDto
{
    public string Code { get; set; } = null!;
}

public class CreateEndpointDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class ActivateEndpointDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeactivateEndpointDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteEndpointDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreEndpointDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}