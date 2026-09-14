namespace Hermes.Application.Operations.DTOs.OperationDtos;

public class CreateOperationCommand
{
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
}

public class ValidateOperationCommand
{
    public Guid OperationId { get; set; }
}

public class AcceptOperationCommand
{
    public Guid OperationId { get; set; }
}

public class RejectOperationCommand
{
    public Guid OperationId { get; set; }
}

public class SendOperationCommand
{
    public Guid OperationId { get; set; }
}


public class DeleteOperationCommand
{
    public Guid OperationId { get; set; }
}

public class RestoreOperationCommand
{
    public Guid OperationId { get; set; }
}