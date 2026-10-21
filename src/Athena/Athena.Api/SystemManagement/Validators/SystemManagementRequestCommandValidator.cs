using Athena.Api.SystemManagement.Requests;
using FluentValidation;

namespace Athena.Api.SystemManagement.Validators;

public class SetSystemDesiredStateRequestValidator    : AbstractValidator<SetSystemDesiredStateRequest>
{
    public SetSystemDesiredStateRequestValidator()
    {
        RuleFor(x => x.DesiredState)
            .IsInEnum()
            .WithMessage("The desired state is invalid.");
    }
}

public class CreateSystemLifecycleOperationRequestValidator    : AbstractValidator<CreateSystemLifecycleOperationRequest>
{
    public CreateSystemLifecycleOperationRequestValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("The lifecycle operation type is invalid.");
    }
}

public class FailSystemLifecycleOperationRequestValidator
    : AbstractValidator<FailSystemLifecycleOperationRequest>
{
    public FailSystemLifecycleOperationRequestValidator()
    {
        RuleFor(x => x.ErrorMessage)
            .NotEmpty()
            .WithMessage("An error message is required.")
            .Must(message => !string.IsNullOrWhiteSpace(message))
            .WithMessage("The error message cannot contain only whitespace.");
    }
}