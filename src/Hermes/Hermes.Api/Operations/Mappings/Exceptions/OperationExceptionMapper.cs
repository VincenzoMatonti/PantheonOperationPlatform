using Hermes.Api.Common;
using Hermes.Api.Exceptions.Mappings;
using Hermes.Api.Exceptions.Models;
using ApplicationOperationExceptions = Hermes.Application.Operations.Exceptions;
using DomainOperationExceptions = Hermes.Domain.Operations.Exceptions;

namespace Hermes.Api.Operations.Mappings.Exceptions;

public class OperationExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApplicationOperationExceptions.OperationException || exception is DomainOperationExceptions.OperationException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is ApplicationOperationExceptions.OperationException applicationException)
            return MapApplicationException(applicationException);

        if (exception is DomainOperationExceptions.OperationException domainException)
            return MapDomainException(domainException);

        throw new ApiExceptionMapperConfigurationException(nameof(OperationExceptionMapper), exception.GetType().Name);
    }

    private static ExceptionMappingResult MapApplicationException(ApplicationOperationExceptions.OperationException exception)
    {
        var errorType = exception.Code switch
        {
            ApplicationOperationExceptions.OperationErrorCode.NotFound => ApiErrorType.NotFound,
            ApplicationOperationExceptions.OperationErrorCode.AlreadyExistsByExternalId => ApiErrorType.Conflict,
            ApplicationOperationExceptions.OperationErrorCode.AlreadyExistsByCorrelationId => ApiErrorType.Conflict,
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
                Entity = ApiErrorEntity.Operation,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(statusCode, errors);
    }

    private static ExceptionMappingResult MapDomainException(DomainOperationExceptions.OperationException exception)
    {
        var errors = new List<ApiErrorResponse>
        {
            new()
            {
                Type = ApiErrorType.Conflict,
                Entity = ApiErrorEntity.Operation,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(StatusCodes.Status409Conflict, errors);
    }
}