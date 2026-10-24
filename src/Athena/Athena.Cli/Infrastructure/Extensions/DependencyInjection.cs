using Athena.Cli.Application.Interfaces.Orchestrations;
using Athena.Cli.Application.Interfaces.Systems;
using Athena.Cli.Infrastructure.Services.Common;
using Athena.Cli.Infrastructure.Services.Orchestrations;
using Athena.Cli.Infrastructure.Services.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Cli.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAthenaCliInfrastructure(this IServiceCollection services, Uri baseAddress)
    {
        services.AddHttpClient<AthenaApiClient>(client => { client.BaseAddress = baseAddress; });
        services.AddTransient<ISystemsRegistry, AthenaSystemsRegistryClient>();
        services.AddTransient<ISystemsManagement, AthenaSystemsManagementClient>();
        services.AddTransient<ISystemsDiagnostic, AthenaSystemsDiagnosticClient>();
        services.AddTransient<IOrchestrationsRegistry, AthenaOrchestrationsRegistryClient>();
        services.AddScoped<IOrchestrationManagement, AthenaOrchestrationManagementClient>();
        services.AddScoped<IOrchestrationSystemManagement, AthenaOrchestrationSystemManagementClient>();
        return services;
    }
}