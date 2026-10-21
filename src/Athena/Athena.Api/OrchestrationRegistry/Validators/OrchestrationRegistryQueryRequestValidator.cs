using Athena.Api.OrchestrationRegistry.Requests;
using FluentValidation;

namespace Athena.Api.OrchestrationRegistry.Validators;

public class GetOrchestrationByNameRequestValidator : AbstractValidator<GetOrchestrationByNameRequest>
{
    public GetOrchestrationByNameRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}