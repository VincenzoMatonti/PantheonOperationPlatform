using FluentValidation;
using Hermes.Api.Operations.Requests;


namespace Hermes.Api.Operations.Validations;

public class CreateOperationTypeValidator : AbstractValidator<CreateOperationTypeRequest>
{
    public CreateOperationTypeValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Operation type code is required.")
            .WithState(_ => OperationTypeValidationErrorCode.CodeRequired);

        RuleFor(x => x.Code)
            .MaximumLength(100)
            .WithMessage("Operation type code cannot exceed 100 characters.")
            .WithState(_ => OperationTypeValidationErrorCode.CodeMaximumLength);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Operation type name is required.")
            .WithState(_ => OperationTypeValidationErrorCode.NameRequired);

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Operation type name cannot exceed 100 characters.")
            .WithState(_ => OperationTypeValidationErrorCode.NameMaximumLength);
    }
}

public class RenameOperationTypeValidator : AbstractValidator<RenameOperationTypeRequest>
{
    public RenameOperationTypeValidator()
    {
        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithMessage("Operation type ID is required.")
            .WithState(_ => OperationTypeValidationErrorCode.OperationTypeIdRequired);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Operation type name is required.")
            .WithState(_ => OperationTypeValidationErrorCode.NameRequired);

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Operation type name cannot exceed 100 characters.")
            .WithState(_ => OperationTypeValidationErrorCode.NameMaximumLength);
    }
}

public class RenameOperationTypeCodeValidator : AbstractValidator<RenameOperationTypeCodeRequest>
{
    public RenameOperationTypeCodeValidator()
    {
        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithMessage("Operation type ID is required.")
            .WithState(_ => OperationTypeValidationErrorCode.OperationTypeIdRequired);

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

public class ActivateOperationTypeValidator : AbstractValidator<ActivateOperationTypeRequest>
{
    public ActivateOperationTypeValidator()
    {
        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithMessage("Operation type ID is required.")
            .WithState(_ => OperationTypeValidationErrorCode.OperationTypeIdRequired);
    }
}
public class DeactivateOperationTypeValidator : AbstractValidator<DeactivateOperationTypeRequest>
{
    public DeactivateOperationTypeValidator()
    {
        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithMessage("Operation type ID is required.")
            .WithState(_ => OperationTypeValidationErrorCode.OperationTypeIdRequired);
    }
}
public class DeleteOperationTypeValidator : AbstractValidator<DeleteOperationTypeRequest>
{
    public DeleteOperationTypeValidator()
    {
        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithMessage("Operation type ID is required.")
            .WithState(_ => OperationTypeValidationErrorCode.OperationTypeIdRequired);
    }
}
public class RestoreOperationTypeValidator : AbstractValidator<RestoreOperationTypeRequest>
{
    public RestoreOperationTypeValidator()
    {
        RuleFor(x => x.OperationTypeId)
            .NotEmpty()
            .WithMessage("Operation type ID is required.")
            .WithState(_ => OperationTypeValidationErrorCode.OperationTypeIdRequired);
    }
}
