namespace Hermes.Api.Operations.Requests;

public class GetOperationByIdRequest
{
    public Guid OperationId { get; set; }
}

public class GetOperationByExternalIdRequest
{
    public string ExternalId { get; set; } = null!;
}

public class GetOperationByCorrelationIdRequest
{
    public Guid CorrelationId { get; set; }
}

public class GetOperationsByStatusRequest
{
    public string Status { get; set; } = null!;
}

public class GetOperationsByExecutionIdRequest
{
    public Guid ExecutionId { get; set; }
}