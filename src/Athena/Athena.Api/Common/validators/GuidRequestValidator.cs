using FluentValidation;

namespace Athena.Api.Common.validators;

public sealed class GuidRequestValidator : AbstractValidator<Guid>
{
    public GuidRequestValidator() =>
        RuleFor(id => id).NotEmpty().WithMessage("The identifier is required.");
}
