using Athena.Application.BackgroundTasks.Synchronization.Interfaces;
using Athena.Application.Common.Interfaces;
using Athena.Application.SystemManagement.Contracts;
using Athena.Domain.Block.Registry.Repositories;
using Athena.Domain.Company.Repositories;
using Athena.Domain.Ecosystem.Registry.Repositories;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.Repositories;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.Workload.Registry.Repositories;
using Athena.Domain.Workload.Management.Repositories;
using Athena.Domain.Block.Management.Repositories;
using Athena.Domain.Ecosystem.Management.Repositories;
using Athena.Infrastructure.Persistence;
using Athena.Infrastructure.Repositories;
using Athena.Infrastructure.Seeding;
using Athena.Infrastructure.Services.ProcessManagement;
using Athena.Infrastructure.Services.Synchronization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Infrastructure.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddAthenaInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddRepositories(services);
        AddExecutionAgent(services);
        AddSynchronizationServices(services);
        AddAthenaSeeder(services);
        return services;
    }

    public static async Task SeedAthenaAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var seeder = scope.ServiceProvider.GetRequiredService<AthenaSeeder>();
        await seeder.SeedAsync(cancellationToken);
    }

    private static IServiceCollection AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Athena");
        if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException("La connection string 'Athena' non è configurata.");
        services.AddDbContext<AthenaDbContext>(options => options.UseNpgsql(connectionString));
        return services;
    }

    private static IServiceCollection AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ISystemRepository, SystemRepository>();
        services.AddScoped<IOrchestrationRepository, OrchestrationRepository>();
        services.AddScoped<IOrchestrationLifecycleRepository, OrchestrationLifecycleRepository>();
        services.AddScoped<IOrchestrationLifecycleOperationRepository, OrchestrationLifecycleOperationRepository>();
        services.AddScoped<IOrchestrationSystemInstanceRepository, OrchestrationSystemInstanceRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IEcosystemRepository, EcosystemRepository>();
        services.AddScoped<IBlockRepository, BlockRepository>();
        services.AddScoped<IWorkloadRepository, WorkloadRepository>();
        services.AddScoped<ISystemInstanceRepository, SystemInstanceRepository>();
        services.AddScoped<ISystemInstanceOperationRepository, SystemInstanceOperationRepository>();
        services.AddScoped<ISystemInstanceLifecycleRepository, SystemInstanceLifecycleRepository>();
        services.AddScoped<IWorkloadLifecycleRepository, WorkloadLifecycleRepository>();
        services.AddScoped<IWorkloadOperationRepository, WorkloadOperationRepository>();
        services.AddScoped<IBlockLifecycleRepository, BlockLifecycleRepository>();
        services.AddScoped<IBlockOperationRepository, BlockOperationRepository>();
        services.AddScoped<IEcosystemLifecycleRepository, EcosystemLifecycleRepository>();
        services.AddScoped<IEcosystemOperationRepository, EcosystemOperationRepository>();
        return services;
    }

    private static IServiceCollection AddExecutionAgent(IServiceCollection services)
    {
        services.AddHttpClient<ISystemInstanceExecutionAgent, SystemInstanceExecutionAgentClient>((sp, client) =>
        {
            client.Timeout = TimeSpan.FromSeconds(15);
        });
        services.AddScoped<ICorrelatedSystemInstanceExecutionAgent>(sp =>
            (ICorrelatedSystemInstanceExecutionAgent)sp.GetRequiredService<ISystemInstanceExecutionAgent>());

        return services;
    }

    private static IServiceCollection AddSynchronizationServices(IServiceCollection services)
    {
        services.AddHttpClient<IEndpointAvailabilityChecker, HttpEndpointAvailabilityChecker>(client => { client.Timeout = TimeSpan.FromSeconds(5); });
        return services;
    }

    private static IServiceCollection AddAthenaSeeder(IServiceCollection services)
    {
        services.AddScoped<AthenaSeeder>();
        return services;
    }
}
