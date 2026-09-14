namespace Hermes.Application.OperationTypes.Commands;

public class CreateOperationTypeCommand
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class RenameOperationTypeCommand
{
    public Guid OperationTypeId { get; set; }
    public string Name { get; set; } = null!;
}

public class RenameOperationTypeCodeCommand
{
    public Guid OperationTypeId { get; set; }
    public string Code { get; set; } = null!;
}

public class ActivateOperationTypeCommand
{
    public Guid OperationTypeId { get; set; }
}

public class DeactivateOperationTypeCommand
{
    public Guid OperationTypeId { get; set; }
}

public class DeleteOperationTypeCommand
{
    public Guid OperationTypeId { get; set; }
}

public class RestoreOperationTypeCommand
{
    public Guid OperationTypeId { get; set; }
}