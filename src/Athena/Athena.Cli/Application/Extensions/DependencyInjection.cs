using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Application.Services.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Cli.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAthenaCliApplication(this IServiceCollection services)
    {
        services.AddTransient<SystemRegistryService>();
        services.AddTransient<SystemManagementService>();
        services.AddTransient<OrchestrationRegistryService>();
        services.AddScoped<OrchestrationManagementService>();
        services.AddScoped<OrchestrationSystemManagementService>();
        return services;
    }
}