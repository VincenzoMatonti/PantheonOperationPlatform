using FluentValidation;
using Hermes.Api.Clients.Requests;

namespace Hermes.Api.Clients.Validations;

public class GetClientOperationByIdRequestValidator : AbstractValidator<GetClientOperationByIdRequest>
{
    public GetClientOperationByIdRequestValidator()
    {
        RuleFor(x => x.ClientOperationId)
            .NotEmpty()
            .WithState(_ => ClientOperationValidationErrorCode.ClientOperationIdRequired)
            .WithMessage("Client operation ID is required.");
    }
}

public class GetClientOperationRequestValidator : AbstractValidator<GetClientOperationRequest>
{
    public GetClientOperationRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithState(_ => ClientOperationValidationErrorCode.ClientIdRequired)
            .WithMessage("Client ID is required.");

        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithState(_ => ClientOperationValidationErrorCode.OperationTypeIdRequired)
            .WithMessage("Operation type ID is required.");
    }
}

public class GetClientOperationsByClientIdRequestValidator : AbstractValidator<GetClientOperationsByClientIdRequest>
{
    public GetClientOperationsByClientIdRequestValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithState(_ => ClientOperationValidationErrorCode.ClientIdRequired)
            .WithMessage("Client ID is required.");
    }
}

public class GetClientOperationsByOperationTypeIdRequestValidator : AbstractValidator<GetClientOperationsByOperationTypeIdRequest>
{
    public GetClientOperationsByOperationTypeIdRequestValidator()
    {
        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithState(_ => ClientOperationValidationErrorCode.OperationTypeIdRequired)
            .WithMessage("Operation type ID is required.");
    }
}