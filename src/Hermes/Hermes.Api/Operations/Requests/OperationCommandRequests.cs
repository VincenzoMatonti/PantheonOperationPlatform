namespace Hermes.Api.Operations.Requests;

public class CreateOperationRequest
{
    public Guid ExecutionId { get; set; }

    public Guid CorrelationId { get; set; }

    public string ExternalId { get; set; } = null!;
}

public class SendOperationRequest
{
    public Guid OperationId { get; set; }
}

public class ValidateOperationRequest
{
    public Guid OperationId { get; set; }
}

public class AcceptOperationRequest
{
    public Guid OperationId { get; set; }
}

public class RejectOperationRequest
{
    public Guid OperationId { get; set; }
}

public class DeleteOperationRequest
{
    public Guid OperationId { get; set; }
}

public class RestoreOperationRequest
{
    public Guid OperationId { get; set; }
}