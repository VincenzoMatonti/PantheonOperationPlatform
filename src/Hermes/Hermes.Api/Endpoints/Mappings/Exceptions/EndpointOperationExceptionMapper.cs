using Hermes.Api.Common;
using Hermes.Api.Exceptions.Mappings;
using Hermes.Api.Exceptions.Models;
using ApplicationEndpointOperationExceptions = Hermes.Application.Endpoints.Exceptions;
using DomainEndpointOperationExceptions = Hermes.Domain.Endpoints.Exceptions;

namespace Hermes.Api.Endpoints.Mappings.Exceptions;

public class EndpointOperationExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApplicationEndpointOperationExceptions.EndpointOperationException ||
               exception is DomainEndpointOperationExceptions.EndpointOperationException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is ApplicationEndpointOperationExceptions.EndpointOperationException applicationException) return MapApplicationException(applicationException);
        if (exception is DomainEndpointOperationExceptions.EndpointOperationException domainException) return MapDomainException(domainException);
        throw new ApiExceptionMapperConfigurationException(nameof(EndpointOperationExceptionMapper), exception.GetType().Name);
    }

    private static ExceptionMappingResult MapApplicationException(ApplicationEndpointOperationExceptions.EndpointOperationException exception)
    {
        var errorType = exception.Code switch
        {
            ApplicationEndpointOperationExceptions.EndpointOperationErrorCode.NotFoundById => ApiErrorType.NotFound,
            ApplicationEndpointOperationExceptions.EndpointOperationErrorCode.NotFoundByEndpointAndOperationType => ApiErrorType.NotFound,
            ApplicationEndpointOperationExceptions.EndpointOperationErrorCode.AlreadyExistsByEndpointAndOperationType => ApiErrorType.Conflict,
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
                Entity = ApiErrorEntity.EndpointOperation,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(statusCode, errors);
    }

    private static ExceptionMappingResult MapDomainException(DomainEndpointOperationExceptions.EndpointOperationException exception)
    {
        var errors = new List<ApiErrorResponse>
        {
            new()
            {
                Type = ApiErrorType.Conflict,
                Entity = ApiErrorEntity.EndpointOperation,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(StatusCodes.Status409Conflict, errors);
    }
}

