using Athena.Cli.Application.Services.Orchestrations;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Application.Services.Workloads;
using Athena.Cli.Application.Services.Blocks;
using Athena.Cli.Application.Services.Ecosystems;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Cli.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAthenaCliApplication(this IServiceCollection services)
    {
        services.AddTransient<SystemRegistryService>();
        services.AddScoped<SystemInstancesService>();
        services.AddTransient<OrchestrationRegistryService>();
        services.AddScoped<OrchestrationManagementService>();
        services.AddScoped<OrchestrationSystemInstanceManagementService>();
        services.AddScoped<WorkloadManagementService>();
        services.AddScoped<BlockManagementService>();
        services.AddScoped<EcosystemManagementService>();
        return services;
    }
}