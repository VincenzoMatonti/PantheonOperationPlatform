using Athena.Api.SystemManagement.Requests;
using FluentValidation;

namespace Athena.Api.SystemManagement.Validators;

public class ReadRuntimeLogsRequestValidator : AbstractValidator<ReadRuntimeLogsRequest>
{
    public ReadRuntimeLogsRequestValidator()
    {
        RuleFor(x => x.Tail).InclusiveBetween(1, 1000).WithMessage("Tail must be between 1 and 1000.");
        RuleFor(x => x)
            .Must(x => !x.SinceUtc.HasValue || !x.UntilUtc.HasValue || x.SinceUtc.Value <= x.UntilUtc.Value)
            .WithMessage("SinceUtc cannot be later than UntilUtc.");
    }
}