using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Athena.Api.Common.extensions;

public static class AthenaApiExtensions
{
    public static IServiceCollection AddAthenaApi(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddSwaggerGen();
        services.AddAthenaApiCommonServices();
        services.AddAthenaApiMiddleware();
        return services;
    }

    public static WebApplication UseAthenaApi(this WebApplication app)
    {
        app.UseAthenaApiMiddleware();
        app.UseHttpsRedirection();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Athena API v1");
                options.RoutePrefix = "swagger";
            });
        }

        app.MapControllers();
        return app;
    }
}