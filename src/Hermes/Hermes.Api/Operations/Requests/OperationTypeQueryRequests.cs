namespace Hermes.Api.Operations.Requests;

public class GetOperationTypeByIdRequest
{
    public Guid OperationTypeId { get; set; }
}

public class GetOperationTypeByCodeRequest
{
    public string Code { get; set; } = null!;
}

