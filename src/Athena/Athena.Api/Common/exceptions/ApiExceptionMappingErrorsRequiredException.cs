using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions.parent;

namespace Athena.Api.Common.exceptions;

public class ApiExceptionMappingErrorsRequiredException : ApiCommonException
{
    public ApiExceptionMappingErrorsRequiredException() : base(
        ApiCommonErrorCode.ExceptionMappingErrorsRequired,
        "At least one API error is required for an exception mapping.")
    {
    }
}