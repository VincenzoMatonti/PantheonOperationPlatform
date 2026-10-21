using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Enums;
using FluentValidation;

namespace Athena.Api.OrchestrationManagement.Validators;

public class CreateOrchestrationLifecycleOperationRequestValidator : AbstractValidator<CreateOrchestrationLifecycleOperationRequest>
{
    public CreateOrchestrationLifecycleOperationRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("The operation ID is required.");

        RuleFor(x => x.Type)
            .Must(value => Enum.TryParse<OrchestrationLifecycleOperationType>(value, true, out var type) && Enum.IsDefined(type))
            .WithMessage("The lifecycle operation type is invalid.");
    }
}

public class FailOrchestrationLifecycleOperationRequestValidator : AbstractValidator<FailOrchestrationLifecycleOperationRequest>
{
    public FailOrchestrationLifecycleOperationRequestValidator()
    {
        RuleFor(x => x.ErrorMessage)
            .NotEmpty()
            .WithMessage("An error message is required.")
            .Must(message => !string.IsNullOrWhiteSpace(message))
            .WithMessage("The error message cannot contain only whitespace.");
    }
}