using Athena.Api.Common.services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Athena.Api.Common.middleware;

public class GlobalExceptionHandler(
    ExceptionHandlingService exceptionHandlingService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ExceptionHandlingService _exceptionHandlingService = exceptionHandlingService;
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Original exception while processing {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);
        var response = _exceptionHandlingService.Handle(exception);
        httpContext.Response.StatusCode = response.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(response.Body, cancellationToken);
        return true;
    }
}
