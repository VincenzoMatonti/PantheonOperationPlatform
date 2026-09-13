using System.Text.Json;
using Hermes.Api.Common.Responses;
using Hermes.Domain.Common;
using Microsoft.AspNetCore.Diagnostics;
using ApplicationClientExceptions = Hermes.Application.Clients.Exceptions;
using DomainClientExceptions = Hermes.Domain.Clients.Exceptions;

namespace Hermes.Api.Common.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var response = CreateResponse(exception);
        if (response.StatusCode >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred. Type: {ExceptionType}", exception.GetType().Name);
        }
        else
        {
            _logger.LogWarning("Handled exception occurred. Type: {ExceptionType}", exception.GetType().Name);
        }
        httpContext.Response.StatusCode = response.StatusCode;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response.Body), cancellationToken);
        return true;
    }

    private static ExceptionResponse CreateResponse(Exception exception)
    {
        if (exception is ApiValidationException validationException)
            return CreateValidationResponse(validationException);

        if (exception is ApiValidationConfigurationException)
            return CreateInternalResponse();

        if (exception is ApplicationClientExceptions.ClientException clientException)
            return CreateClientResponse(clientException);


        if (exception is ApplicationClientExceptions.ClientOperationException clientOperationException)
            return CreateClientOperationResponse(clientOperationException);


        if (exception is DomainException<DomainClientExceptions.ClientErrorCode> clientDomainException)
            return CreateClientDomainResponse(clientDomainException);

        if (exception is DomainException<DomainClientExceptions.ClientOperationErrorCode> clientOperationDomainException)
            return CreateClientOperationDomainResponse(clientOperationDomainException);

        return CreateInternalResponse();
    }

    private static ExceptionResponse CreateValidationResponse(ApiValidationException exception)
    {
        var errors = exception.Errors.Select(error => new ApiErrorResponse
        {
            Type = ApiErrorType.Validation,
            Entity = ApiErrorEntity.Unknown,
            Code = error.Code.ToString(),
            Message = error.Message
        })
        .ToList();

        var response = ApiResponse.Fail(errors);
        return new ExceptionResponse(StatusCodes.Status400BadRequest, response);
    }

    private static ExceptionResponse CreateClientResponse(ApplicationClientExceptions.ClientException exception)
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

        var response = ApiResponse.Fail(errorType, ApiErrorEntity.Client, exception.Code.ToString(), exception.Message);
        return new ExceptionResponse(statusCode, response);
    }

    private static ExceptionResponse CreateClientOperationResponse(ApplicationClientExceptions.ClientOperationException exception)
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

        var response = ApiResponse.Fail(errorType, ApiErrorEntity.ClientOperation, exception.Code.ToString(), exception.Message);
        return new ExceptionResponse(statusCode, response);
    }

    private static ExceptionResponse CreateClientDomainResponse(DomainException<DomainClientExceptions.ClientErrorCode> exception)
    {
        var response = ApiResponse.Fail(ApiErrorType.Conflict, ApiErrorEntity.Client, exception.Code.ToString(), exception.Message);
        return new ExceptionResponse(StatusCodes.Status409Conflict, response);
    }

    private static ExceptionResponse CreateClientOperationDomainResponse(DomainException<DomainClientExceptions.ClientOperationErrorCode> exception)
    {
        var response = ApiResponse.Fail(ApiErrorType.Conflict, ApiErrorEntity.ClientOperation, exception.Code.ToString(), exception.Message);
        return new ExceptionResponse(StatusCodes.Status409Conflict, response);
    }

    private static ExceptionResponse CreateInternalResponse()
    {
        var response = ApiResponse.Fail(ApiErrorType.Internal, ApiErrorEntity.Unknown, "InternalError", "An internal error occurred.");
        return new ExceptionResponse(StatusCodes.Status500InternalServerError, response);
    }

    private sealed record ExceptionResponse(int StatusCode, object Body);
}