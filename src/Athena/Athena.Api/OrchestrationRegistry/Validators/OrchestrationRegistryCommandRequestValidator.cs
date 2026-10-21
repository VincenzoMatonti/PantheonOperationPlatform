using Athena.Api.OrchestrationRegistry.Requests;
using FluentValidation;

namespace Athena.Api.OrchestrationRegistry.Validators;

public class CreateOrchestrationRequestValidator : AbstractValidator<CreateOrchestrationRequest>
{
    public CreateOrchestrationRequestValidator()
    {
        RuleFor(x => x.OrchestrationId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}

public class RenameOrchestrationRequestValidator : AbstractValidator<RenameOrchestrationRequest>
{
    public RenameOrchestrationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}