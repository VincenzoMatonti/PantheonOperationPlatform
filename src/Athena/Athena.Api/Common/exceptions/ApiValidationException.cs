using FluentValidation.Results;
using Athena.Api.Common.exceptions.parent;
using Athena.Api.Common.enums;

namespace Athena.Api.Common.exceptions;

public sealed class ApiValidationException : ApiCommonException
{
    public IReadOnlyList<ValidationFailure> Errors { get; }

    public ApiValidationException(IEnumerable<ValidationFailure> errors) : base(ApiCommonErrorCode.ValidationFailed, "Request validation failed.")
    {
        ArgumentNullException.ThrowIfNull(errors);
        Errors = [.. errors];
        if (Errors.Count == 0) throw new ArgumentException("At least one validation error is required.", nameof(errors));
    }
}