using Athena.Api.WorkloadManagement.Requests;
using Athena.Domain.Workload.Management.Enums;
using FluentValidation;

namespace Athena.Api.WorkloadManagement.Validators;

public sealed class SetWorkloadDesiredStateRequestValidator : AbstractValidator<SetWorkloadDesiredStateRequest>
{
    public SetWorkloadDesiredStateRequestValidator() =>
        RuleFor(request => request.DesiredState)
            .Must(value => Enum.TryParse<WorkloadDesiredState>(value, true, out var state) && Enum.IsDefined(state))
            .WithMessage("The desired state is invalid.");
}

public sealed class WorkloadOperationTypeValidator : AbstractValidator<WorkloadOperationType>
{
    public WorkloadOperationTypeValidator() =>
        RuleFor(operation => operation).Must(Enum.IsDefined).WithMessage("The operation type is invalid.");
}
