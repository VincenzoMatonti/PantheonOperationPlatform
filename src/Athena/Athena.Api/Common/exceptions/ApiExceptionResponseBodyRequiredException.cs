using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions.parent;

namespace Athena.Api.Common.exceptions;

public sealed class ApiExceptionResponseBodyRequiredException : ApiCommonException
{
    public ApiExceptionResponseBodyRequiredException() : base(
        ApiCommonErrorCode.ExceptionResponseBodyRequired,
        "An API response body is required.")
    {
    }
}