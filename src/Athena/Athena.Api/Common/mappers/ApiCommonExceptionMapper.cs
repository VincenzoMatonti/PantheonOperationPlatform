using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.exceptions.parent;
using Athena.Api.Common.interfaces;

namespace Athena.Api.Common.mappers;

public sealed class ApiCommonExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApiCommonException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not ApiCommonException commonException)
            throw new ApiArgumentException("The exception is not an Athena API common exception.", nameof(exception));

        return new ExceptionMappingResult(StatusCodes.Status500InternalServerError,
        [
            new ApiErrorResponse
            {
                Type = ApiErrorType.Internal,
                Entity = ApiErrorEntity.Unknown,
                Code = commonException.Code.ToString(),
                Message = "An unexpected error occurred."
            }
        ]);
    }
}