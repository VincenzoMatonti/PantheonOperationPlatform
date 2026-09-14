using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Application.Operations.DTOs.OperationDtos;

public class OperationDto
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
}

public class CreateOperationDto
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class ValidateOperationDto
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class AcceptOperationDto
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RejectOperationDto
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class SendOperationDto
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class DeleteOperationDto
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RestoreOperationDto
{
    public Guid Id { get; set; }
    public Guid ExecutionId { get; set; }
    public Guid CorrelationId { get; set; }
    public string ExternalId { get; set; } = null!;
    public OperationStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}