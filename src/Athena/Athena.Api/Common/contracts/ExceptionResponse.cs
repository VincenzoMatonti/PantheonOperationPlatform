using Athena.Api.Common.exceptions;

namespace Athena.Api.Common.contracts;

public sealed class ExceptionResponse
{
    public int StatusCode { get; }
    public ApiResponse Body { get; }
    public ExceptionResponse(int statusCode, ApiResponse body)
    {
        if (statusCode is < 400 or > 599) throw new ApiInvalidExceptionMappingStatusCodeException(statusCode);
        if (body is null) throw new ApiExceptionResponseBodyRequiredException();
        StatusCode = statusCode;
        Body = body;
    }
}