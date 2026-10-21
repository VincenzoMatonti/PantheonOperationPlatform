using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.interfaces;
using Athena.Application.Common.exceptions;
using Microsoft.AspNetCore.Http;

namespace Athena.Api.Common.mappers;

public class ApiApplicationCoreExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApplicationCoreException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not ApplicationCoreException applicationException)
            throw new ApiArgumentException("The exception is not an application core exception.", nameof(exception));

        var (statusCode, errorType, entity) = MapErrorType(applicationException.ErrorType);
        return new ExceptionMappingResult(statusCode,
            [
                new ApiErrorResponse
                {
                    Type = errorType,
                    Entity = entity,
                    Code = applicationException.Code,
                    Message = statusCode == StatusCodes.Status500InternalServerError ? "An unexpected error occurred." : applicationException.Message
                }
            ]);
    }

    private static (int StatusCode, ApiErrorType ErrorType, ApiErrorEntity Entity) MapErrorType(ApplicationCoreErrorType errorType)
    {
        return errorType switch
        {
            ApplicationCoreErrorType.Validation => (StatusCodes.Status400BadRequest, ApiErrorType.Validation, ApiErrorEntity.Request),
            ApplicationCoreErrorType.NotFound => (StatusCodes.Status404NotFound, ApiErrorType.NotFound, ApiErrorEntity.Resource),
            ApplicationCoreErrorType.Conflict => (StatusCodes.Status409Conflict, ApiErrorType.Conflict, ApiErrorEntity.Resource),
            ApplicationCoreErrorType.Internal => (StatusCodes.Status500InternalServerError, ApiErrorType.Internal, ApiErrorEntity.Unknown),
            _ => (StatusCodes.Status500InternalServerError, ApiErrorType.Internal, ApiErrorEntity.Unknown)
        };
    }
}