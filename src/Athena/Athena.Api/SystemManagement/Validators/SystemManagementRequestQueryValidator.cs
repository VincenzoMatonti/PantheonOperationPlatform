using Athena.Api.SystemManagement.Requests;
using FluentValidation;

namespace Athena.Api.SystemManagement.Validators;

public class GetSystemLifecyclesByDesiredStateRequestValidator : AbstractValidator<GetSystemLifecyclesByDesiredStateRequest>
{
    public GetSystemLifecyclesByDesiredStateRequestValidator()
    {
        RuleFor(x => x.DesiredState)
            .IsInEnum()
            .WithMessage("The desired state filter is invalid.");
    }
}

public class GetSystemLifecyclesByOperationalStatusRequestValidator : AbstractValidator<GetSystemLifecyclesByOperationalStatusRequest>
{
    public GetSystemLifecyclesByOperationalStatusRequestValidator()
    {
        RuleFor(x => x.OperationalStatus)
            .IsInEnum()
            .WithMessage("The operational status filter is invalid.");
    }
}

public class GetSystemLifecycleOperationsByStatusRequestValidator : AbstractValidator<GetSystemLifecycleOperationsByStatusRequest>
{
    public GetSystemLifecycleOperationsByStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("The lifecycle operation status filter is invalid.");
    }
}

public class GetSystemLifecycleOperationsBySystemIdAndStatusRequestValidator : AbstractValidator<GetSystemLifecycleOperationsBySystemIdAndStatusRequest>
{
    public GetSystemLifecycleOperationsBySystemIdAndStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("The lifecycle operation status filter is invalid.");
    }
}