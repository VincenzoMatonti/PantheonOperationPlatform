using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Api.Operations.Responses;

public class CreateOperationResponse
{
    public Guid Id { get; set; }

    public Guid ExecutionId { get; set; }

    public Guid CorrelationId { get; set; }

    public string ExternalId { get; set; } = null!;

    public OperationStatus Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}

public class SendOperationResponse
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class ValidateOperationResponse
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class AcceptOperationResponse
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RejectOperationResponse
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteOperationResponse
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreOperationResponse
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}