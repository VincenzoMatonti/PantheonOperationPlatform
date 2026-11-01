using Athena.Cli.Application.Interfaces.Orchestrations;
using Athena.Cli.Application.Interfaces.Systems;
using Athena.Cli.Infrastructure.Services.Common;
using Athena.Cli.Infrastructure.Services.Orchestrations;
using Athena.Cli.Infrastructure.Services.Systems;
using Athena.Cli.Application.Interfaces.Workloads;
using Athena.Cli.Application.Interfaces.Blocks;
using Athena.Cli.Application.Interfaces.Ecosystems;
using Athena.Cli.Infrastructure.Services.Workloads;
using Athena.Cli.Infrastructure.Services.Blocks;
using Athena.Cli.Infrastructure.Services.Ecosystems;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Cli.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAthenaCliInfrastructure(this IServiceCollection services, Uri baseAddress)
    {
        services.AddHttpClient<AthenaApiClient>(client => { client.BaseAddress = baseAddress; });
        services.AddTransient<ISystemsRegistry, AthenaSystemsRegistryClient>();
        services.AddTransient<ISystemInstances, AthenaSystemInstancesClient>();
        services.AddTransient<IOrchestrationsRegistry, AthenaOrchestrationsRegistryClient>();
        services.AddScoped<IOrchestrationManagement, AthenaOrchestrationManagementClient>();
        services.AddScoped<IOrchestrationSystemInstanceManagement, AthenaOrchestrationSystemInstanceManagementClient>();
        services.AddScoped<IWorkloadManagement, AthenaWorkloadManagementClient>();
        services.AddScoped<IBlockManagement, AthenaBlockManagementClient>();
        services.AddScoped<IEcosystemManagement, AthenaEcosystemManagementClient>();
        return services;
    }
}