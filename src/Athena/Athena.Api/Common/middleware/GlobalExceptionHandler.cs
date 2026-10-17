using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Athena.Api.Common.services;

public sealed class GlobalExceptionHandler(ExceptionHandlingService exceptionHandlingService) : IExceptionHandler
{
    private readonly ExceptionHandlingService _exceptionHandlingService = exceptionHandlingService;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var response = _exceptionHandlingService.Handle(exception);
        httpContext.Response.StatusCode = response.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(response.Body, cancellationToken);
        return true;
    }
}
