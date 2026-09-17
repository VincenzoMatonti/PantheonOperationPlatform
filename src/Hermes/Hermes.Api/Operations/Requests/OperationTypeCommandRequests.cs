namespace Hermes.Api.Operations.Requests;

public class CreateOperationTypeRequest
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}

public class RenameOperationTypeRequest
{
    public Guid OperationTypeId { get; set; }
    public string Name { get; set; } = null!;
}

public class RenameOperationTypeCodeRequest
{
    public Guid OperationTypeId { get; set; }
    public string Code { get; set; } = null!;
}

public class ActivateOperationTypeRequest
{
    public Guid OperationTypeId { get; set; }
}

public class DeactivateOperationTypeRequest
{
    public Guid OperationTypeId { get; set; }
}

public class DeleteOperationTypeRequest
{
    public Guid OperationTypeId { get; set; }
}

public class RestoreOperationTypeRequest
{
    public Guid OperationTypeId { get; set; }
}