using Athena.Api.EcosystemManagement.Requests;
using Athena.Domain.Ecosystem.Management.Enums;
using FluentValidation;

namespace Athena.Api.EcosystemManagement.Validators;

public sealed class SetEcosystemDesiredStateRequestValidator : AbstractValidator<SetEcosystemDesiredStateRequest>
{
    public SetEcosystemDesiredStateRequestValidator() =>
        RuleFor(request => request.DesiredState)
            .Must(value => Enum.TryParse<EcosystemDesiredState>(value, true, out var state) && Enum.IsDefined(state))
            .WithMessage("The desired state is invalid.");
}

public sealed class EcosystemOperationTypeValidator : AbstractValidator<EcosystemOperationType>
{
    public EcosystemOperationTypeValidator() =>
        RuleFor(operation => operation).Must(Enum.IsDefined).WithMessage("The operation type is invalid.");
}
