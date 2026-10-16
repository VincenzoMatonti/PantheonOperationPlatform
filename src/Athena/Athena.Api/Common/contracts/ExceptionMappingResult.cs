using Athena.Api.Common.exceptions;

namespace Athena.Api.Common.contracts;

public sealed class ExceptionMappingResult
{
    public int StatusCode { get; }
    public IReadOnlyList<ApiErrorResponse> Errors { get; }
    public ExceptionMappingResult(int statusCode, IReadOnlyList<ApiErrorResponse> errors)
    {
        if (statusCode is < 400 or > 599) throw new ApiInvalidExceptionMappingStatusCodeException(statusCode);
        if (errors is null) throw new ApiArgumentNullException(nameof(errors));
        if (errors.Count == 0) throw new ApiExceptionMappingErrorsRequiredException();
        StatusCode = statusCode;
        Errors = errors;
    }
}