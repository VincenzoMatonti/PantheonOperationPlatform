
using Athena.Api.OrchestrationManagement.Requests;
using FluentValidation;

namespace Athena.Api.OrchestrationManagement.Validators;

public class CreateOrchestrationSystemRequestValidator : AbstractValidator<CreateOrchestrationSystemRequest>
{
    public CreateOrchestrationSystemRequestValidator()
    {
        RuleFor(x => x.OrchestrationId)
            .NotEmpty()
            .WithMessage("The orchestration ID is required.");

        RuleFor(x => x.SystemId)
            .NotEmpty()
            .WithMessage("The system ID is required.");
    }
}
