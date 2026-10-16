using Athena.Api.Common.exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Athena.Api.Common.services;

public sealed class RequestValidationService(IEnumerable<IValidator> validators)
{
    private readonly IReadOnlyList<IValidator> _validators = validators?.ToArray() ?? throw new ApiArgumentNullException(nameof(validators));

    public async Task<ValidationResult> ValidateAsync(object request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ApiArgumentNullException(nameof(request));
        var failures = new List<ValidationFailure>();
        var applicableValidators = _validators.Where(validator => validator.CanValidateInstancesOfType(request.GetType()));
        foreach (var validator in applicableValidators)
        {
            var context = new ValidationContext<object>(request);
            var result = await validator.ValidateAsync(context, cancellationToken);
            failures.AddRange(result.Errors);
        }

        return new ValidationResult(failures);
    }
}
