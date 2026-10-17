using Athena.Api.Common.builders;
using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.exceptions;
using Athena.Api.Common.exceptions.parent;
using Athena.Api.Common.resolvers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Athena.Api.Common.services;

public sealed class ExceptionHandlingService(
    ExceptionMapperResolver resolver,
    ExceptionResponseBuilder responseBuilder,
    ILogger<ExceptionHandlingService> logger)
{
    private readonly ExceptionMapperResolver _resolver = resolver;
    private readonly ExceptionResponseBuilder _responseBuilder = responseBuilder;
    private readonly ILogger<ExceptionHandlingService> _logger = logger;

    public ExceptionResponse Handle(Exception exception)
    {
        try
        {
            var mappingResult = _resolver.Map(exception);
            var response = _responseBuilder.Build(mappingResult);
            LogException(exception, response.StatusCode);
            return response;
        }
        catch (ApiExceptionMapperNotFoundException mappingException)
        {
            _logger.LogError(mappingException, "No API exception mapper found for {ExceptionType}.", exception.GetType().FullName);
            return CreateInternalErrorResponse();
        }
        catch (ApiCommonException mappingException)
        {
            _logger.LogError(mappingException, "An Athena API common exception occurred while handling {ExceptionType}.", exception.GetType().FullName);
            return CreateInternalErrorResponse();
        }
        catch (Exception mappingException)
        {
            _logger.LogError(mappingException, "Unexpected error while mapping exception {ExceptionType}.", exception.GetType().FullName);
            return CreateInternalErrorResponse();
        }
    }

    private void LogException(Exception exception, int statusCode)
    {
        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "An exception was mapped to HTTP {StatusCode}.", statusCode);
            return;
        }

        _logger.LogWarning("An exception was mapped to HTTP {StatusCode}. Exception: {ExceptionType}.", statusCode, exception.GetType().FullName);
    }

    private static ExceptionResponse CreateInternalErrorResponse()
    {
        var body = ApiResponse.Fail(ApiErrorType.Internal, ApiErrorEntity.Unknown, "INTERNAL_SERVER_ERROR", "An unexpected error occurred.");
        return new ExceptionResponse(StatusCodes.Status500InternalServerError, body);
    }
}
