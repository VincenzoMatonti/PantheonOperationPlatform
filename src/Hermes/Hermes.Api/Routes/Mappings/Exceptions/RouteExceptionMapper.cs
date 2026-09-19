using Hermes.Api.Common;
using Hermes.Api.Exceptions.Mappings;
using Hermes.Api.Exceptions.Models;
using ApplicationRouteExceptions = Hermes.Application.Routes.Exceptions;
using DomainRouteExceptions = Hermes.Domain.Routes.Exceptions;

namespace Hermes.Api.Routes.Mappings.Exceptions;

public class RouteExceptionMapper : IExceptionMapper
{
    public bool CanHandle(Exception exception)
    {
        return exception is ApplicationRouteExceptions.RouteException || exception is DomainRouteExceptions.RouteException;
    }


    public ExceptionMappingResult Map(Exception exception)
    {
        if (exception is ApplicationRouteExceptions.RouteException applicationException) return MapApplicationException(applicationException);
        if (exception is DomainRouteExceptions.RouteException domainException) return MapDomainException(domainException);
        throw new ApiExceptionMapperConfigurationException(nameof(RouteExceptionMapper), exception.GetType().Name);
    }

    private static ExceptionMappingResult MapApplicationException(ApplicationRouteExceptions.RouteException exception)
    {
        var errorType = exception.Code switch
        {
            ApplicationRouteExceptions.RouteErrorCode.NotFound => ApiErrorType.NotFound,
            ApplicationRouteExceptions.RouteErrorCode.AlreadyExists => ApiErrorType.Conflict,
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
                Entity = ApiErrorEntity.Route,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(statusCode, errors);
    }

    private static ExceptionMappingResult MapDomainException(DomainRouteExceptions.RouteException exception)
    {
        var errors = new List<ApiErrorResponse>
        {
            new()
            {
                Type = ApiErrorType.Conflict,
                Entity = ApiErrorEntity.Route,
                Code = exception.Code.ToString(),
                Message = exception.Message
            }
        };

        return new ExceptionMappingResult(StatusCodes.Status409Conflict, errors);
    }


}
