using Athena.Application.SystemManagement.Commands;
using Athena.Application.SystemManagement.Queries;
using Athena.Application.SystemManagement.UseCases;
using Athena.Application.SystemRegistry.Commands;
using Athena.Application.SystemRegistry.Queries;
using Athena.Application.SystemRegistry.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Application.Common.Extensions;

public static class AthenaApplicationExtensions
{
    public static IServiceCollection AddAthenaApplication(this IServiceCollection services)
    {
        AddSystemRegistryApplicationServices(services);
        AddSystemManagementApplicationServices(services);
        return services;
    }

    private static void AddSystemRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<SystemRegistryCommandHandler>();
        services.AddScoped<SystemRegistryQueryHandler>();
        services.AddScoped<SystemRegistryUseCaseHandler>();
    }

    private static void AddSystemManagementApplicationServices(IServiceCollection services)
    {
        services.AddScoped<SystemManagementCommandHandler>();
        services.AddScoped<SystemManagementQueryHandler>();
        services.AddScoped<SystemManagementUseCaseHandler>();
    }
}