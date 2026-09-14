using Hermes.Api.Common;
using Hermes.Api.Exceptions.Models;
using Hermes.Api.Exceptions.Mappings;

namespace Hermes.Api.Clients.Mappings.Exceptions;

public class ValidationExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApiValidationException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is not ApiValidationException validationException)
            throw new ApiExceptionMapperConfigurationException(nameof(ValidationExceptionMapper), exception.GetType().Name);


        var errors = validationException.Errors.Select(error => new ApiErrorResponse
        {
            Type = ApiErrorType.Validation,
            Entity = ApiErrorEntity.Unknown,
            Code = error.Code.ToString(),
            Message = error.Message
        })
        .ToList();

        return new ExceptionMappingResult(StatusCodes.Status400BadRequest, errors);
    }
}