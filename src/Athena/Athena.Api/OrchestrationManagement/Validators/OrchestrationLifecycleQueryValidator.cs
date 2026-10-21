using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Enums;
using FluentValidation;

namespace Athena.Api.OrchestrationManagement.Validators;

public class GetOrchestrationLifecyclesByDesiredStateRequestValidator : AbstractValidator<GetOrchestrationLifecyclesByDesiredStateRequest>
{
    public GetOrchestrationLifecyclesByDesiredStateRequestValidator()
    {
        RuleFor(x => x.State)
            .Must(value => Enum.TryParse<OrchestrationDesiredState>(value, true, out var state) && Enum.IsDefined(state))
            .WithMessage("The desired state filter is invalid.");
    }
}

public class GetOrchestrationLifecyclesByOperationalStatusRequestValidator : AbstractValidator<GetOrchestrationLifecyclesByOperationalStatusRequest>
{
    public GetOrchestrationLifecyclesByOperationalStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .Must(value => Enum.TryParse<OrchestrationOperationalStatus>(value, true, out var status) && Enum.IsDefined(status))
            .WithMessage("The operational status filter is invalid.");
    }
}