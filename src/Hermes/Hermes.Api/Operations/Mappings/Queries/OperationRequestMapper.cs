using Hermes.Api.Operations.Requests;
using Hermes.Application.Operations.DTOs.OperationDtos;
using Hermes.Domain.Operations.ValueObjects;

namespace Hermes.Api.Operations.Mappings.Queries;

public class OperationQueryRequestMapper
{
    public GetOperationByIdQuery ToQuery(GetOperationByIdRequest request)
    {
        return new GetOperationByIdQuery
        {
            OperationId = request.OperationId
        };
    }

    public GetOperationByExternalIdQuery ToQuery(GetOperationByExternalIdRequest request)
    {
        return new GetOperationByExternalIdQuery
        {
            ExternalId = request.ExternalId
        };
    }

    public GetOperationByCorrelationIdQuery ToQuery(GetOperationByCorrelationIdRequest request)
    {
        return new GetOperationByCorrelationIdQuery
        {
            CorrelationId = request.CorrelationId
        };
    }

    public GetOperationsByStatusQuery ToQuery(GetOperationsByStatusRequest request)
    {
        if (!Enum.TryParse<OperationStatus>(
                request.Status,
                true,
                out var status))
        {
            throw new ArgumentException(
                $"Invalid operation status: {request.Status}");
        }

        return new GetOperationsByStatusQuery
        {
            Status = status
        };
    }

    public GetOperationsByExecutionIdQuery ToQuery(GetOperationsByExecutionIdRequest request)
    {
        return new GetOperationsByExecutionIdQuery
        {
            ExecutionId = request.ExecutionId
        };
    }
}