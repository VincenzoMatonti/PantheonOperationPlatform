using Hermes.Api.Common;
using Hermes.Api.Exceptions.Mappings;
using Hermes.Api.Exceptions.Models;
using ApplicationOperationTypeExceptions = Hermes.Application.Operations.Exceptions;
using DomainOperationTypeExceptions = Hermes.Domain.Operations.Exceptions;

namespace Hermes.Api.Operations.Mappings.Exceptions;

public class OperationTypeExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApplicationOperationTypeExceptions.OperationTypeException || exception is DomainOperationTypeExceptions.OperationTypeException;
    }

    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is ApplicationOperationTypeExceptions.OperationTypeException applicationException)
            return MapApplicationException(applicationException);

        if (exception is DomainOperationTypeExceptions.OperationTypeException domainException)
            return MapDomainException(domainException);

        throw new ApiExceptionMapperConfigurationException(nameof(OperationTypeExceptionMapper), exception.GetType().Name);
    }

    private static ExceptionMappingResult MapApplicationException(ApplicationOperationTypeExceptions.OperationTypeException exception)
    {
        var errorType = exception.Code switch
        {
            ApplicationOperationTypeExceptions.OperationTypeErrorCode.NotFound => ApiErrorType.NotFound,
            ApplicationOperationTypeExceptions.OperationTypeErrorCode.AlreadyExists => ApiErrorType.Conflict,
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
                Entity = ApiErrorEntity.OperationType,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(statusCode, errors);
    }

    private static ExceptionMappingResult MapDomainException(DomainOperationTypeExceptions.OperationTypeException exception)
    {
        var errors = new List<ApiErrorResponse>
        {
            new()
            {
                Type = ApiErrorType.Conflict,
                Entity = ApiErrorEntity.OperationType,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(StatusCodes.Status409Conflict, errors);
    }
}
