using Athena.Application.BackgroundTasks.Synchronization.Interfaces;
using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Interfaces.Runtime;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.Repositories;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.System.Registry.Repositories;
using Athena.Infrastructure.Persistence;
using Athena.Infrastructure.Repositories;
using Athena.Infrastructure.Seeding;
using Athena.Infrastructure.Services.Runtime;
using Athena.Infrastructure.Services.Runtime.Providers.Docker;
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
        AddRuntimeServices(services);
        AddSynchronizationServices(services);
        AddAthenaSeeder(services);
        return services;
    }

    public static async Task InitializeAthenaDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AthenaDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);
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
        services.AddScoped<ISystemLifecycleRepository, SystemLifecycleRepository>();
        services.AddScoped<ISystemLifecycleOperationRepository, SystemLifecycleOperationRepository>();
        services.AddScoped<IOrchestrationRepository, OrchestrationRepository>();
        services.AddScoped<IOrchestrationLifecycleRepository, OrchestrationLifecycleRepository>();
        services.AddScoped<IOrchestrationLifecycleOperationRepository, OrchestrationLifecycleOperationRepository>();
        services.AddScoped<IOrchestrationSystemRepository, OrchestrationSystemRepository>();
        return services;
    }

    private static IServiceCollection AddRuntimeServices(IServiceCollection services)
    {
        services.AddScoped<ISystemRuntimeInstanceRegistry, SystemRuntimeInstanceRegistry>();
        services.AddScoped<IRuntimeProviderResolver, RuntimeProviderResolver>();
        services.AddScoped<IRuntimeProvider, DockerRuntimeProvider>();
        services.AddScoped<IRuntimeHealthCheck, DockerRuntimeHealthCheck>();
        services.AddScoped<IRuntimeLogReader, DockerRuntimeLogReader>();
        services.AddScoped<IRuntimeDiscoveryService, DockerRuntimeDiscoveryService>();
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
