using System.Text.Json;
using Hermes.Api.Exceptions.Builders;
using Hermes.Api.Exceptions.Mappings;
using Hermes.Api.Exceptions.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace Hermes.Api.Exceptions.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, ExceptionMapperResolver resolver, ExceptionResponseBuilder builder) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    private readonly ExceptionMapperResolver _resolver = resolver;
    private readonly ExceptionResponseBuilder _builder = builder;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        try
        {
            var mapper = _resolver.Resolve(exception);
            var mappingResult = mapper.Map(exception);
            var response = _builder.Build(mappingResult);
            LogException(exception, response.StatusCode);
            httpContext.Response.StatusCode = response.StatusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response.Body), cancellationToken);
            return true;
        }
        catch (Exception handlerException)
        {
            _logger.LogError(handlerException, "An error occurred while handling exception of type {ExceptionType}.", exception.GetType().Name);
            var internalException = new ApiInternalException();
            var mapper = _resolver.Resolve(internalException);
            var mappingResult = mapper.Map(internalException);
            var response = _builder.Build(mappingResult);
            httpContext.Response.StatusCode = response.StatusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response.Body), cancellationToken);
            return true;
        }
    }

    private void LogException(Exception exception, int statusCode)
    {
        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred. Type: {ExceptionType}.", exception.GetType().Name);
            return;
        }

        _logger.LogWarning("Handled exception occurred. Type: {ExceptionType}. StatusCode: {StatusCode}.", exception.GetType().Name, statusCode);
    }
}