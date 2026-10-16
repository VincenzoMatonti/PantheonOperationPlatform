
using Athena.Api.SystemRegistry.Requests;
using FluentValidation;

namespace Athena.Api.SystemRegistry.Validators;

public class GetSystemByNameRequestValidator : AbstractValidator<GetSystemByNameRequest>
{
    public GetSystemByNameRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}

public sealed class GetSystemsByCapabilityRequestValidator : AbstractValidator<GetSystemsByCapabilityRequest>
{
    public GetSystemsByCapabilityRequestValidator()
    {
        RuleFor(x => x.CapabilityName)
            .NotEmpty()
            .MaximumLength(200);
    }
}
