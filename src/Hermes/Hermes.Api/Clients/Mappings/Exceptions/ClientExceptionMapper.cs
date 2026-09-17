using Hermes.Api.Common;
using Hermes.Api.Exceptions.Mappings;
using Hermes.Api.Exceptions.Models;
using ApplicationClientExceptions = Hermes.Application.Clients.Exceptions;
using DomainClientExceptions = Hermes.Domain.Clients.Exceptions;

namespace Hermes.Api.Clients.Mappings.Exceptions;

public class ClientExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApplicationClientExceptions.ClientException || exception is DomainClientExceptions.ClientException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is ApplicationClientExceptions.ClientException applicationException)
            return MapApplicationException(applicationException);

        if (exception is DomainClientExceptions.ClientException domainException)
            return MapDomainException(domainException);

        throw new ApiExceptionMapperConfigurationException(nameof(ClientExceptionMapper), exception.GetType().Name);
    }

    private static ExceptionMappingResult MapApplicationException(ApplicationClientExceptions.ClientException exception)
    {
        var errorType = exception.Code switch
        {
            ApplicationClientExceptions.ClientErrorCode.NotFound => ApiErrorType.NotFound,
            ApplicationClientExceptions.ClientErrorCode.AlreadyExists => ApiErrorType.Conflict,
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
                Entity = ApiErrorEntity.Client,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(statusCode, errors);
    }

    private static ExceptionMappingResult MapDomainException(DomainClientExceptions.ClientException exception)
    {
        var errors = new List<ApiErrorResponse>
        {
            new(){
                Type = ApiErrorType.Conflict,
                Entity = ApiErrorEntity.Client,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(StatusCodes.Status409Conflict, errors);
    }
}