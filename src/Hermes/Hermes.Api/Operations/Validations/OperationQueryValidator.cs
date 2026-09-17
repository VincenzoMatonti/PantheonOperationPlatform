using FluentValidation;
using Hermes.Api.Operations.Requests;

namespace Hermes.Api.Operations.Validations.Queries;

public class GetOperationByIdRequestValidator : AbstractValidator<GetOperationByIdRequest>
{
    public GetOperationByIdRequestValidator()
    {
        RuleFor(x => x.OperationId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.OperationIdRequired)
            .WithMessage("Operation ID is required.");
    }
}

public class GetOperationByExternalIdRequestValidator : AbstractValidator<GetOperationByExternalIdRequest>
{
    public GetOperationByExternalIdRequestValidator()
    {
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

public class GetOperationByCorrelationIdRequestValidator : AbstractValidator<GetOperationByCorrelationIdRequest>
{
    public GetOperationByCorrelationIdRequestValidator()
    {
        RuleFor(x => x.CorrelationId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.CorrelationIdRequired)
            .WithMessage("Correlation ID is required.");
    }
}

public class GetOperationsByStatusRequestValidator : AbstractValidator<GetOperationsByStatusRequest>
{
    public GetOperationsByStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.StatusRequired)
            .WithMessage("Operation status is required.");
    }
}

public class GetOperationsByExecutionIdRequestValidator : AbstractValidator<GetOperationsByExecutionIdRequest>
{
    public GetOperationsByExecutionIdRequestValidator()
    {
        RuleFor(x => x.ExecutionId)
            .NotEmpty()
            .WithState(_ => OperationValidationErrorCode.ExecutionIdRequired)
            .WithMessage("Execution ID is required.");
    }
}