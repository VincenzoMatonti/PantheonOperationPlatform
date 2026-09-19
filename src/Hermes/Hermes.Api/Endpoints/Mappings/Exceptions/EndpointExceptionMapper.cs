using Hermes.Api.Common;
using Hermes.Api.Exceptions.Mappings;
using Hermes.Api.Exceptions.Models;
using ApplicationEndpointExceptions = Hermes.Application.Endpoints.Exceptions;
using DomainEndpointExceptions = Hermes.Domain.Endpoints.Exceptions;

namespace Hermes.Api.Endpoints.Mappings.Exceptions;

public class EndpointExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApplicationEndpointExceptions.EndpointException || exception is DomainEndpointExceptions.EndpointException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is ApplicationEndpointExceptions.EndpointException applicationException) return MapApplicationException(applicationException);
        if (exception is DomainEndpointExceptions.EndpointException domainException) return MapDomainException(domainException);
        throw new ApiExceptionMapperConfigurationException(nameof(EndpointExceptionMapper), exception.GetType().Name);
    }

    private static ExceptionMappingResult MapApplicationException(ApplicationEndpointExceptions.EndpointException exception)
    {
        var errorType = exception.Code switch
        {
            ApplicationEndpointExceptions.EndpointErrorCode.NotFoundById => ApiErrorType.NotFound,
            ApplicationEndpointExceptions.EndpointErrorCode.NotFoundByCode => ApiErrorType.NotFound,
            ApplicationEndpointExceptions.EndpointErrorCode.NotFoundByType => ApiErrorType.NotFound,
            ApplicationEndpointExceptions.EndpointErrorCode.AlreadyExistsByCode => ApiErrorType.Conflict,
            ApplicationEndpointExceptions.EndpointErrorCode.AlreadyExistsByType => ApiErrorType.Conflict,
            _ => ApiErrorType.Internal
        };

        var statusCode = errorType switch
        {
            ApiErrorType.NotFound => StatusCodes.Status404NotFound,
            ApiErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var errors = new List<ApiErrorResponse>
        {
            new()
            {
                Type = errorType,
                Entity = ApiErrorEntity.Endpoint,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(statusCode, errors);
    }

    private static ExceptionMappingResult MapDomainException(DomainEndpointExceptions.EndpointException exception)
    {
        var errors = new List<ApiErrorResponse>
        {
            new()
            {
                Type = ApiErrorType.Conflict,
                Entity = ApiErrorEntity.Endpoint,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(StatusCodes.Status409Conflict, errors);
    }
}

