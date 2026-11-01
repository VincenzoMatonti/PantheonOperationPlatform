using Athena.Api.OrchestrationRegistry.Requests;
using Athena.Application.OrchestrationRegistry.Enums;
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

public sealed class OrchestrationRegistrationStatusFilterValidator : AbstractValidator<OrchestrationRegistrationStatusFilter>
{
    public OrchestrationRegistrationStatusFilterValidator() =>
        RuleFor(status => status).Must(Enum.IsDefined).WithMessage("The registration status is invalid.");
}

public sealed class OrchestrationAvailabilityStatusFilterValidator : AbstractValidator<OrchestrationAvailabilityStatusFilter>
{
    public OrchestrationAvailabilityStatusFilterValidator() =>
        RuleFor(status => status).Must(Enum.IsDefined).WithMessage("The availability status is invalid.");
}