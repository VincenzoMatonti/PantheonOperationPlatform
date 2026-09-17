using Hermes.Api.Common;
using Hermes.Api.Exceptions.Mappings;
using Hermes.Api.Exceptions.Models;
using ApplicationClientExceptions = Hermes.Application.Clients.Exceptions;
using DomainClientExceptions = Hermes.Domain.Clients.Exceptions;

namespace Hermes.Api.Clients.Mappings.Exceptions;

public class ClientOperationExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApplicationClientExceptions.ClientOperationException || exception is DomainClientExceptions.ClientOperationException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is ApplicationClientExceptions.ClientOperationException applicationException)
            return MapApplicationException(applicationException);

        if (exception is DomainClientExceptions.ClientOperationException domainException)
            return MapDomainException(domainException);

        throw new ApiExceptionMapperConfigurationException(nameof(ClientOperationExceptionMapper), exception.GetType().Name);
    }

    private static ExceptionMappingResult MapApplicationException(ApplicationClientExceptions.ClientOperationException exception)
    {
        var errorType = exception.Code switch
        {
            ApplicationClientExceptions.ClientOperationErrorCode.NotFound => ApiErrorType.NotFound,
            ApplicationClientExceptions.ClientOperationErrorCode.AlreadyExists => ApiErrorType.Conflict,
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
            new(){
                Type = errorType,
                Entity = ApiErrorEntity.ClientOperation,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(statusCode, errors);
    }

    private static ExceptionMappingResult MapDomainException(DomainClientExceptions.ClientOperationException exception)
    {
        var errors = new List<ApiErrorResponse>
        {
            new(){
                Type = ApiErrorType.Conflict,
                Entity = ApiErrorEntity.ClientOperation,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(StatusCodes.Status409Conflict, errors);
    }
}