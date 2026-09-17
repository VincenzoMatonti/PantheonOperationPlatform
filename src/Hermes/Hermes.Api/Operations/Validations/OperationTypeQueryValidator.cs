using FluentValidation;
using Hermes.Api.Operations.Requests;


namespace Hermes.Api.Operations.Validations;

public class GetOperationTypeByIdValidator : AbstractValidator<GetOperationTypeByIdRequest>
{
    public GetOperationTypeByIdValidator()
    {
        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithMessage("Operation type ID is required.")
            .WithState(_ => OperationTypeValidationErrorCode.OperationTypeIdRequired);
    }
}

public class GetOperationTypeByCodeValidator : AbstractValidator<GetOperationTypeByCodeRequest>
{
    public GetOperationTypeByCodeValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Operation type code is required.")
            .WithState(_ => OperationTypeValidationErrorCode.CodeRequired);

        RuleFor(x => x.Code)
            .MaximumLength(100)
            .WithMessage("Operation type code cannot exceed 100 characters.")
            .WithState(_ => OperationTypeValidationErrorCode.CodeMaximumLength);
    }
}