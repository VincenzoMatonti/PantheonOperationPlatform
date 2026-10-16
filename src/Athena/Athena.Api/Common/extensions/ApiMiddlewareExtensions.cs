using Athena.Api.Common.middleware;
using Athena.Api.Common.services;

namespace Athena.Api.Common.extensions;

public static class ApiMiddlewareExtensions
{
    public static IServiceCollection AddAthenaApiMiddleware(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
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
