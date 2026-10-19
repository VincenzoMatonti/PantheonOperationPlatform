using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Mappings;
using Athena.Application.Common.Services;
using Athena.Application.OrchestrationRegistry.Commands;
using Athena.Application.OrchestrationRegistry.Queries;
using Athena.Application.OrchestrationRegistry.UseCases;
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
        AddOrchestrationRegistryApplicationServices(services);
        AddApplicationExceptionHandling(services);
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

     private static void AddOrchestrationRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<OrchestrationRegistryCommandHandler>();
        services.AddScoped<OrchestrationRegistryQueryHandler>();
        services.AddScoped<OrchestrationRegistryUseCaseHandler>();
    }

    private static IServiceCollection AddApplicationExceptionHandling(IServiceCollection services)
    {
        services.AddSingleton<IApplicationExceptionMapper, DomainExceptionMapper>();
        services.AddSingleton<IApplicationExceptionMapper, ApplicationExceptionMapper>();
        services.AddSingleton<ApplicationExceptionService>();
        return services;
    }
}