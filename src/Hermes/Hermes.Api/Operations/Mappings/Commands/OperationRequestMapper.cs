using Hermes.Api.Operations.Requests;
using Hermes.Application.Operations.DTOs.OperationDtos;

namespace Hermes.Api.Operations.Mappings.Commands;

public class OperationCommandRequestMapper
{
    public CreateOperationCommand ToCommand(CreateOperationRequest request)
    {
        return new CreateOperationCommand
        {
            ExecutionId = request.ExecutionId,
            CorrelationId = request.CorrelationId,
            ExternalId = request.ExternalId
        };
    }

    public SendOperationCommand ToCommand(SendOperationRequest request)
    {
        return new SendOperationCommand
        {
            OperationId = request.OperationId
        };
    }

    public ValidateOperationCommand ToCommand(ValidateOperationRequest request)
    {
        return new ValidateOperationCommand
        {
            OperationId = request.OperationId
        };
    }

    public AcceptOperationCommand ToCommand(AcceptOperationRequest request)
    {
        return new AcceptOperationCommand
        {
            OperationId = request.OperationId
        };
    }

    public RejectOperationCommand ToCommand(RejectOperationRequest request)
    {
        return new RejectOperationCommand
        {
            OperationId = request.OperationId
        };
    }

    public DeleteOperationCommand ToCommand(DeleteOperationRequest request)
    {
        return new DeleteOperationCommand
        {
            OperationId = request.OperationId
        };
    }

    public RestoreOperationCommand ToCommand(RestoreOperationRequest request)
    {
        return new RestoreOperationCommand
        {
            OperationId = request.OperationId
        };
    }
}