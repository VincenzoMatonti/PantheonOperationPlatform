using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Athena.Infrastructure.Persistence;
using Athena.Infrastructure.Repositories;
using Athena.Domain.SystemRegistry.Repositories;


namespace Athena.Infrastructure.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddAthenaInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Athena");
        if (string.IsNullOrWhiteSpace(connectionString)) throw new InvalidOperationException("La connection string 'Athena' non è configurata.");
        services.AddDbContext<AthenaDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Athena")));
        services.AddScoped<ISystemRepository, SystemRepository>();
        return services;
    }
}