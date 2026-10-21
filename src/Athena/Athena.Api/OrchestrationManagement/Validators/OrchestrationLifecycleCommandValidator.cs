using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Enums;
using FluentValidation;

namespace Athena.Api.OrchestrationManagement.Validators;

public class SetOrchestrationDesiredStateRequestValidator : AbstractValidator<SetOrchestrationDesiredStateRequest>
{
    public SetOrchestrationDesiredStateRequestValidator()
    {
        RuleFor(x => x.DesiredState)
            .Must(value => Enum.TryParse<OrchestrationDesiredState>(value, true, out var state) && Enum.IsDefined(state))
            .WithMessage("The desired state is invalid.");
    }
}