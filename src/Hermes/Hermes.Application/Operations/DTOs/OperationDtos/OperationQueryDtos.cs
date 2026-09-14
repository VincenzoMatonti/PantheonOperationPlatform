using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Application.Operations.DTOs.OperationDtos;

public class GetOperationByIdQuery
{
    public Guid OperationId { get; set; }
}

public class GetOperationByExternalIdQuery
{
    public string ExternalId { get; set; } = null!;
}

public class GetOperationByCorrelationIdQuery
{
    public Guid CorrelationId { get; set; }
}

public class GetOperationsByStatusQuery
{
    public OperationStatus Status { get; set; }
}

public class GetOperationsByExecutionIdQuery
{
    public Guid ExecutionId { get; set; }
}