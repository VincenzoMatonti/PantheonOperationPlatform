using Athena.Api.Common.contracts;
using Athena.Api.Common.enums;
using Athena.Api.Common.middleware;
using Athena.Api.Common.services;
using Athena.Api.SystemRegistry.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Api.Common.extensions;

public static class ApiMiddlewareExtensions
{
    public static IServiceCollection AddAthenaApiMiddleware(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddValidatorsFromAssemblyContaining<RegisterSystemRequestValidator>();
        services.AddScoped<RequestValidationService>();
        services.AddControllers(options => { options.Filters.Add<RequestValidationFilter>(); })
        .AddApplicationPart(typeof(RegisterSystemRequestValidator).Assembly)
        .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()); })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Where(entry => entry.Value?.Errors.Count > 0).SelectMany(entry => entry.Value!.Errors.Select(error =>
                new ApiErrorResponse
                {
                    Type = ApiErrorType.Validation,
                    Entity = ApiErrorEntity.Request,
                    Code = "VALIDATION_ERROR",
                    Message = string.IsNullOrWhiteSpace(entry.Key) ? error.ErrorMessage : $"{entry.Key}: {error.ErrorMessage}"
                }))
                .ToList();

                return new BadRequestObjectResult(ApiResponse.Fail(errors));
            };
        });


        return services;
    }

    public static WebApplication UseAthenaApiMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler();
        return app;
    }
}
