using Hermes.Api.Common;
using Hermes.Api.Exceptions.Models;

namespace Hermes.Api.Exceptions.Mappings;

public class InternalExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApiInternalException
            || exception is ApiValidationConfigurationException
            || exception is ApiExceptionMapperNotFoundException
            || exception is ApiExceptionMapperConfigurationException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (!CanHandle(exception))
        {
            throw new ApiExceptionMapperConfigurationException(nameof(InternalExceptionMapper), exception.GetType().Name);
        }

        var error = new ApiErrorResponse
        {
            Type = ApiErrorType.Internal,
            Entity = ApiErrorEntity.Unknown,
            Code = GetErrorCode(exception),
            Message = "An internal server error occurred."
        };

        return new ExceptionMappingResult(StatusCodes.Status500InternalServerError, [error]);
    }

    private static string GetErrorCode(Exception exception)
    {
        return exception switch
        {
            ApiValidationConfigurationException => "ValidationConfigurationError",
            ApiExceptionMapperNotFoundException => "ExceptionMapperNotFound",
            ApiExceptionMapperConfigurationException => "ExceptionMapperConfigurationError",
            ApiInternalException => "InternalError",
            _ => "InternalError"
        };
    }
}