using Athena.Infrastructure.Persistence;
using Athena.Infrastructure.Repositories;
using Athena.Domain.OrchestrationRegistry.Repositories;
using Athena.Domain.SystemManagement.Repositories;
using Athena.Domain.SystemRegistry.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Athena.Domain.OrchestrationManagement.Repositories;

namespace Athena.Infrastructure.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddAthenaInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDatabase(services, configuration);
        AddRepositories(services);
        return services;
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
}
