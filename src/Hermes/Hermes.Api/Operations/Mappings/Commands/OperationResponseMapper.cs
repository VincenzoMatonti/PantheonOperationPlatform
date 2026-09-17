using Hermes.Api.Operations.Responses;
using Hermes.Application.Operations.DTOs.OperationDtos;

namespace Hermes.Api.Operations.Mappings.Commands;

public class OperationCommandResponseMapper
{
    public CreateOperationResponse ToResponse(CreateOperationDto operation)
    {
        return new CreateOperationResponse
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId,
            ExternalId = operation.ExternalId,
            Status = operation.Status,
            CreatedAt = operation.CreatedAt
        };
    }

    public SendOperationResponse ToResponse(SendOperationDto operation)
    {
        return new SendOperationResponse
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId,
            ExternalId = operation.ExternalId,
            Status = operation.Status,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public ValidateOperationResponse ToResponse(ValidateOperationDto operation)
    {
        return new ValidateOperationResponse
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId,
            ExternalId = operation.ExternalId,
            Status = operation.Status,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public AcceptOperationResponse ToResponse(AcceptOperationDto operation)
    {
        return new AcceptOperationResponse
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId,
            ExternalId = operation.ExternalId,
            Status = operation.Status,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public RejectOperationResponse ToResponse(RejectOperationDto operation)
    {
        return new RejectOperationResponse
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId,
            ExternalId = operation.ExternalId,
            Status = operation.Status,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public DeleteOperationResponse ToResponse(DeleteOperationDto operation)
    {
        return new DeleteOperationResponse
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId,
            ExternalId = operation.ExternalId,
            Status = operation.Status,
            IsDeleted = operation.IsDeleted,
            UpdatedAt = operation.UpdatedAt
        };
    }

    public RestoreOperationResponse ToResponse(RestoreOperationDto operation)
    {
        return new RestoreOperationResponse
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId,
            ExternalId = operation.ExternalId,
            Status = operation.Status,
            IsDeleted = operation.IsDeleted,
            UpdatedAt = operation.UpdatedAt
        };
    }
}