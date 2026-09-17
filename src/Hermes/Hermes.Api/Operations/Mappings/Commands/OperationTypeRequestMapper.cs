using Hermes.Api.Operations.Requests;
using Hermes.Application.OperationTypes.Commands;

namespace Hermes.Api.Operations.Mappings.Commands;

public class OperationTypeCommandRequestMapper
{
    public CreateOperationTypeCommand ToCommand(CreateOperationTypeRequest request)
        => new()
        {
            Code = request.Code,
            Name = request.Name
        };

    public RenameOperationTypeCommand ToCommand(RenameOperationTypeRequest request)
        => new()
        {
            OperationTypeId = request.OperationTypeId,
            Name = request.Name
        };

    public RenameOperationTypeCodeCommand ToCommand(RenameOperationTypeCodeRequest request)
        => new()
        {
            OperationTypeId = request.OperationTypeId,
            Code = request.Code
        };

    public ActivateOperationTypeCommand ToCommand(ActivateOperationTypeRequest request)
        => new()
        {
            OperationTypeId = request.OperationTypeId
        };

    public DeactivateOperationTypeCommand ToCommand(DeactivateOperationTypeRequest request)
        => new()
        {
            OperationTypeId = request.OperationTypeId
        };

    public DeleteOperationTypeCommand ToCommand(DeleteOperationTypeRequest request)
        => new()
        {
            OperationTypeId = request.OperationTypeId
        };

    public RestoreOperationTypeCommand ToCommand(RestoreOperationTypeRequest request)
        => new()
        {
            OperationTypeId = request.OperationTypeId
        };
}

