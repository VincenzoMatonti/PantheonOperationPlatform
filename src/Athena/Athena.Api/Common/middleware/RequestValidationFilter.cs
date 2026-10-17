
using Athena.Api.Common.exceptions;
using Athena.Api.Common.services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Athena.Api.Common.middleware;

public class RequestValidationFilter(RequestValidationService requestValidationService) : IAsyncActionFilter
{
    private readonly RequestValidationService _requestValidationService = requestValidationService;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;
            var result = await _requestValidationService.ValidateAsync(argument, context.HttpContext.RequestAborted);
            if (result.Errors.Count > 0) throw new ApiValidationException(result.Errors);
        }

        await next();
    }
}
