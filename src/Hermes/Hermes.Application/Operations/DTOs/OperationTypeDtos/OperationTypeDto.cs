namespace Hermes.Application.OperationTypes.DTOs.OperationTypeDtos;

public class OperationTypeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class CreateOperationTypeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class RenameOperationTypeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RenameOperationTypeCodeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset UpdatedAt { get; set; }
}

public class ActivateOperationTypeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeactivateOperationTypeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteOperationTypeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreOperationTypeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}