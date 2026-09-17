using Hermes.Api.Operations.Responses;
using Hermes.Application.OperationTypes.DTOs.OperationTypeDtos;

namespace Hermes.Api.Operations.Mappings.Queries;

public class OperationTypeQueryResponseMapper
{
    public OperationTypeResponse ToResponse(OperationTypeDto operationType)
        => new()
        {
            Id = operationType.Id,
            Code = operationType.Code,
            Name = operationType.Name
        };

    public List<OperationTypeResponse> ToResponse(List<OperationTypeDto> operationTypes)
        => [.. operationTypes.Select(ToResponse)];
}
