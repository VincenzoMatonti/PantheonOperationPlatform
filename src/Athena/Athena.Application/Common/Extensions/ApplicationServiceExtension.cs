using Athena.Application.BackgroundTasks.Reconciliation;
using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Mappings;
using Athena.Application.Common.Services;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.OrchestrationManagement.UseCases;
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
        AddApplicationService(services);
        AddApplicationExceptionHandling(services);
        AddBackgroundService(services);
        return services;
    }

    //private method
    private static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        AddSystemRegistryApplicationServices(services);
        AddSystemManagementApplicationServices(services);
        AddOrchestrationRegistryApplicationServices(services);
        AddOrchestrationManagementApplicationServices(services);
        AddRuntimeApplicationServices(services);
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
        services.AddScoped<SystemDiagnosticUseCaseHandler>();
        services.AddScoped<SystemRuntimeUseCaseHandler>();
    }

    private static void AddOrchestrationRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<OrchestrationRegistryCommandHandler>();
        services.AddScoped<OrchestrationRegistryQueryHandler>();
        services.AddScoped<OrchestrationRegistryUseCaseHandler>();
    }

    private static void AddOrchestrationManagementApplicationServices(IServiceCollection services)
    {
        services.AddScoped<OrchestrationLifecycleCommandHandler>();
        services.AddScoped<OrchestrationLifecycleQueryHandler>();
        services.AddScoped<OrchestrationLifecycleUseCaseHandler>();
        services.AddScoped<OrchestrationLifecycleOperationCommandHandler>();
        services.AddScoped<OrchestrationLifecycleOperationQueryHandler>();
        services.AddScoped<OrchestrationLifecycleOperationUseCaseHandler>();        
        services.AddScoped<OrchestrationDesiredStateUseCaseHandler>();
        services.AddScoped<OrchestrationSystemCommandHandler>();
        services.AddScoped<OrchestrationSystemQueryHandler>();
        services.AddScoped<OrchestrationSystemUseCaseHandler>();
        services.AddScoped<OrchestrationStatusSynchronizationService>();
        services.AddScoped<OrchestrationRuntimeUseCaseHandler>();
    }

    private static void AddRuntimeApplicationServices(IServiceCollection services)
    {
        services.AddScoped<RuntimeApplicationService>();
    }

    private static IServiceCollection AddApplicationExceptionHandling(IServiceCollection services)
    {
        services.AddSingleton<IApplicationExceptionMapper, DomainExceptionMapper>();
        services.AddSingleton<IApplicationExceptionMapper, ApplicationExceptionMapper>();
        services.AddSingleton<ApplicationExceptionService>();
        return services;
    }

    private static IServiceCollection AddBackgroundService(IServiceCollection services)
    {
        services.AddScoped<OrchestrationReconciliationService>();
        return services;
    }
}