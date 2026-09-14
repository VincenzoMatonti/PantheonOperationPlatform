namespace Hermes.Application.OperationTypes.Queries;

public class GetOperationTypeByIdQuery
{
    public Guid OperationTypeId { get; set; }
}

public class GetOperationTypeByCodeQuery
{
    public string Code { get; set; } = null!;
}