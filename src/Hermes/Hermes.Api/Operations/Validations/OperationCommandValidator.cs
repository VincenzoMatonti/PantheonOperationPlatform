using FluentValidation;
using Hermes.Api.Operations.Requests;

namespace Hermes.Api.Operations.Validations.Commands;

public class CreateOperationRequestValidator : AbstractValidator<CreateOperationRequest>
{
    public CreateOperationRequestValidator()
    {
        RuleFor(x => x.ExecutionId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.ExecutionIdRequired)
            .WithMessage("Execution ID is required.");

        RuleFor(x => x.CorrelationId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.CorrelationIdRequired)
            .WithMessage("Correlation ID is required.");

        RuleFor(x => x.ExternalId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.ExternalIdRequired)
            .WithMessage("External operation ID is required.")
            .MaximumLength(100)
            .WithState(_ => OperationValidationErrorCode.ExternalIdMaximumLength)
            .WithMessage(
                "External operation ID cannot exceed 100 characters.");
    }
}

public class SendOperationRequestValidator : AbstractValidator<SendOperationRequest>
{
    public SendOperationRequestValidator()
    {
        RuleFor(x => x.OperationId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.OperationIdRequired)
            .WithMessage("Operation ID is required.");
    }
}

public class ValidateOperationRequestValidator : AbstractValidator<ValidateOperationRequest>
{
    public ValidateOperationRequestValidator()
    {
        RuleFor(x => x.OperationId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.OperationIdRequired)
            .WithMessage("Operation ID is required.");
    }
}

public class AcceptOperationRequestValidator : AbstractValidator<AcceptOperationRequest>
{
    public AcceptOperationRequestValidator()
    {
        RuleFor(x => x.OperationId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.OperationIdRequired)
            .WithMessage("Operation ID is required.");
    }
}

public class RejectOperationRequestValidator : AbstractValidator<RejectOperationRequest>
{
    public RejectOperationRequestValidator()
    {
        RuleFor(x => x.OperationId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.OperationIdRequired)
            .WithMessage("Operation ID is required.");
    }
}

public class DeleteOperationRequestValidator : AbstractValidator<DeleteOperationRequest>
{
    public DeleteOperationRequestValidator()
    {
        RuleFor(x => x.OperationId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.OperationIdRequired)
            .WithMessage("Operation ID is required.");
    }
}

public class RestoreOperationRequestValidator : AbstractValidator<RestoreOperationRequest>
{
    public RestoreOperationRequestValidator()
    {
        RuleFor(x => x.OperationId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.OperationIdRequired)
            .WithMessage("Operation ID is required.");
    }
}