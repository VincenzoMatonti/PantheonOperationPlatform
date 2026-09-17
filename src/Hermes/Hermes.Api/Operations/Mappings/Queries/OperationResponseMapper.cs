using Hermes.Api.Operations.Responses;
using Hermes.Application.Operations.DTOs.OperationDtos;

namespace Hermes.Api.Operations.Mappings.Queries;

public class OperationQueryResponseMapper
{
    public OperationResponse ToResponse(OperationDto operation)
    {
        return new OperationResponse
        {
            Id = operation.Id,
            ExecutionId = operation.ExecutionId,
            CorrelationId = operation.CorrelationId,
            ExternalId = operation.ExternalId,
            Status = operation.Status
        };
    }

    public List<OperationResponse> ToResponse(List<OperationDto> operations)
    {
        return [.. operations.Select(ToResponse)];
    }
}