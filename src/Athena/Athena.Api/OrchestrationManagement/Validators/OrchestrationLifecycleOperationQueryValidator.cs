using Athena.Api.OrchestrationManagement.Requests;
using Athena.Application.OrchestrationManagement.Enums;
using FluentValidation;

namespace Athena.Api.OrchestrationManagement.Validators;

public class GetOrchestrationLifecycleOperationsByOrchestrationIdRequestValidator
    : AbstractValidator<GetOrchestrationLifecycleOperationsByOrchestrationIdRequest>
{
    public GetOrchestrationLifecycleOperationsByOrchestrationIdRequestValidator()
    {
        RuleFor(x => x.OrchestrationId).NotEmpty().WithMessage("The orchestration ID is required.");
    }
}

public class GetOrchestrationLifecycleOperationsByTypeRequestValidator : AbstractValidator<GetOrchestrationLifecycleOperationsByTypeRequest>
{
    public GetOrchestrationLifecycleOperationsByTypeRequestValidator()
    {
        RuleFor(x => x.Type)
            .Must(value => Enum.TryParse<OrchestrationLifecycleOperationType>(value, true, out var type) && Enum.IsDefined(type))
            .WithMessage("The lifecycle operation type filter is invalid.");
    }
}

public class GetOrchestrationLifecycleOperationsByStatusRequestValidator : AbstractValidator<GetOrchestrationLifecycleOperationsByStatusRequest>
{
    public GetOrchestrationLifecycleOperationsByStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .Must(value => Enum.TryParse<OrchestrationLifecycleOperationStatus>(value, true, out var status) && Enum.IsDefined(status))
            .WithMessage("The lifecycle operation status filter is invalid.");
    }
}

public class GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusRequestValidator
    : AbstractValidator<GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusRequest>
{
    public GetOrchestrationLifecycleOperationsByOrchestrationIdAndStatusRequestValidator()
    {
        RuleFor(x => x.OrchestrationId)
            .NotEmpty()
            .WithMessage("The orchestration ID is required.");

        RuleFor(x => x.Status)
            .Must(value => Enum.TryParse<OrchestrationLifecycleOperationStatus>(value, true, out var status) && Enum.IsDefined(status))
            .WithMessage("The lifecycle operation status filter is invalid.");
    }
}