
using Athena.Api.OrchestrationManagement.Requests;
using FluentValidation;

namespace Athena.Api.OrchestrationManagement.Validators;

public class CreateOrchestrationSystemInstanceRequestValidator : AbstractValidator<CreateOrchestrationSystemInstanceRequest>
{
    public CreateOrchestrationSystemInstanceRequestValidator()
    {
        RuleFor(x => x.OrchestrationId)
            .NotEmpty()
            .WithMessage("The orchestration ID is required.");

        RuleFor(x => x.SystemInstanceId)
            .NotEmpty()
            .WithMessage("The system ID is required.");
    }
}
