using Athena.Cli.Application.Services.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Cli.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAthenaCliApplication(this IServiceCollection services)
    {
        services.AddTransient<SystemRegistryService>();
        return services;
    }
}