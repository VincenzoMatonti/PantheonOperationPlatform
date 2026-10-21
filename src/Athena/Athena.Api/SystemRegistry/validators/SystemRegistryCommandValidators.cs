
using Athena.Api.SystemRegistry.Requests;
using FluentValidation;

namespace Athena.Api.SystemRegistry.Validators;

public class RegisterSystemRequestValidator : AbstractValidator<RegisterSystemRequest>
{
    public RegisterSystemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}

public sealed class RenameSystemRequestValidator : AbstractValidator<RenameSystemRequest>
{
    public RenameSystemRequestValidator()
    {
        RuleFor(x => x.NewName)
            .NotEmpty()
            .MaximumLength(200);
    }
}

public sealed class AddSystemEndpointRequestValidator : AbstractValidator<AddSystemEndpointRequest>
{
    public AddSystemEndpointRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.BaseAddress)
            .NotEmpty()
            .Must(value => Uri.TryCreate(value, UriKind.Absolute, out _))
            .WithMessage("BaseAddress must be an absolute URI.")
            .MaximumLength(2048);
    }
}

public sealed class RemoveSystemEndpointRequestValidator : AbstractValidator<RemoveSystemEndpointRequest>
{
    public RemoveSystemEndpointRequestValidator()
    {
        RuleFor(x => x.EndpointName)
            .NotEmpty()
            .MaximumLength(200);
    }
}

public sealed class AddSystemCapabilityRequestValidator : AbstractValidator<AddSystemCapabilityRequest>
{
    public AddSystemCapabilityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);
    }
}

public sealed class RemoveSystemCapabilityRequestValidator : AbstractValidator<RemoveSystemCapabilityRequest>
{
    public RemoveSystemCapabilityRequestValidator()
    {
        RuleFor(x => x.CapabilityName)
            .NotEmpty()
            .MaximumLength(200);
    }
}
