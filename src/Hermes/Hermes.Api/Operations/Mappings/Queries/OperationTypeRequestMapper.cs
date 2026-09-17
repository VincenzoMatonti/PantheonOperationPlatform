using Hermes.Api.Operations.Requests;
using Hermes.Application.OperationTypes.Queries;

namespace Hermes.Api.Operations.Mappings.Queries;

public class OperationTypeQueryRequestMapper
{
    public GetOperationTypeByIdQuery ToQuery(GetOperationTypeByIdRequest request)
        => new()
        {
            OperationTypeId = request.OperationTypeId
        };

    public GetOperationTypeByCodeQuery ToQuery(GetOperationTypeByCodeRequest request)
        => new()
        {
            Code = request.Code
        };
}
