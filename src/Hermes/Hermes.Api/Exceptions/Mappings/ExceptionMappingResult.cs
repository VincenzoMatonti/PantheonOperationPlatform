using Hermes.Api.Common;

namespace Hermes.Api.Exceptions.Mappings;

public class ExceptionMappingResult(int statusCode, List<ApiErrorResponse> errors)
{
    public int StatusCode { get; } = statusCode;
    public List<ApiErrorResponse> Errors { get; } = errors;
}