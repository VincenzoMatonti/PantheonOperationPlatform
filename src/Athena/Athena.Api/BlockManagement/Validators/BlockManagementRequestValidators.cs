using Athena.Api.BlockManagement.Requests;
using Athena.Domain.Block.Management.Enums;
using FluentValidation;

namespace Athena.Api.BlockManagement.Validators;

public sealed class SetBlockDesiredStateRequestValidator : AbstractValidator<SetBlockDesiredStateRequest>
{
    public SetBlockDesiredStateRequestValidator() =>
        RuleFor(request => request.DesiredState)
            .Must(value => Enum.TryParse<BlockDesiredState>(value, true, out var state) && Enum.IsDefined(state))
            .WithMessage("The desired state is invalid.");
}

public sealed class BlockOperationTypeValidator : AbstractValidator<BlockOperationType>
{
    public BlockOperationTypeValidator() =>
        RuleFor(operation => operation).Must(Enum.IsDefined).WithMessage("The operation type is invalid.");
}
