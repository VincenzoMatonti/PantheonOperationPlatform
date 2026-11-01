
using Athena.Api.SystemRegistry.Requests;
using Athena.Application.SystemRegistry.Enums;
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

public sealed class SystemRegistrationStatusFilterValidator : AbstractValidator<SystemRegistrationStatusFilter>
{
    public SystemRegistrationStatusFilterValidator() =>
        RuleFor(status => status).Must(Enum.IsDefined).WithMessage("The registration status is invalid.");
}

public sealed class SystemAvailabilityStatusFilterValidator : AbstractValidator<SystemAvailabilityStatusFilter>
{
    public SystemAvailabilityStatusFilterValidator() =>
        RuleFor(status => status).Must(Enum.IsDefined).WithMessage("The availability status is invalid.");
}
