using Athena.Api.Common.middleware;
using Athena.Api.Common.services;
using Athena.Api.SystemRegistry.Validators;
using FluentValidation;

namespace Athena.Api.Common.extensions;

public static class ApiMiddlewareExtensions
{
    public static IServiceCollection AddAthenaApiMiddleware(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddValidatorsFromAssemblyContaining<RegisterSystemRequestValidator>();
        services.AddScoped<RequestValidationService>();
        services.AddControllers(options =>
        {
            options.Filters.Add<RequestValidationFilter>();
        });

        return services;
    }

    public static WebApplication UseAthenaApiMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler();
        return app;
    }
}
