using FluentValidation;
using Hermes.Api.Exceptions.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hermes.Api.Filters;

public class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var validationErrors = new List<ValidationError>();
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;
            var argumentType = argument.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator) continue;
            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);
            foreach (var failure in result.Errors)
            {
                if (failure.CustomState is not Enum code) throw new ApiValidationConfigurationException(failure.PropertyName);
                var validationError = new ValidationError(failure.PropertyName, code, failure.ErrorMessage);
                validationErrors.Add(validationError);
            }
        }

        if (validationErrors.Count > 0) throw new ApiValidationException(validationErrors);
        await next();
    }
}