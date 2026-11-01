using Athena.Application.BackgroundTasks.Operations;
using Athena.Application.BackgroundTasks.Reconciliation;
using Athena.Application.BackgroundTasks.Synchronization;
using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Mappings;
using Athena.Application.Common.Services;
using Athena.Application.CompanyRegistry.Commands;
using Athena.Application.CompanyRegistry.Queries;
using Athena.Application.CompanyRegistry.UseCases;
using Athena.Application.GroupRegistry.Commands;
using Athena.Application.GroupRegistry.Queries;
using Athena.Application.GroupRegistry.UseCases;
using Athena.Application.SystemRegistry.Commands;
using Athena.Application.SystemRegistry.Queries;
using Athena.Application.SystemRegistry.UseCases;
using Athena.Application.SystemInstanceManagement.Services;
using Athena.Application.SystemInstanceManagement.UseCases;
using Athena.Application.OrchestrationRegistry.Commands;
using Athena.Application.OrchestrationRegistry.Queries;
using Athena.Application.OrchestrationRegistry.UseCases;
using Athena.Application.OrchestrationManagement.Commands;
using Athena.Application.OrchestrationManagement.Interfaces;
using Athena.Application.OrchestrationManagement.Queries;
using Athena.Application.OrchestrationManagement.Services;
using Athena.Application.OrchestrationManagement.UseCases;
using Athena.Application.EcosystemRegistry.Commands;
using Athena.Application.EcosystemRegistry.Queries;
using Athena.Application.EcosystemRegistry.UseCases;
using Athena.Application.EcosystemManagement.Commands;
using Athena.Application.EcosystemManagement.Contracts;
using Athena.Application.EcosystemManagement.Queries;
using Athena.Application.EcosystemManagement.UseCases;
using Athena.Application.BlockRegistry.Commands;
using Athena.Application.BlockRegistry.Queries;
using Athena.Application.BlockRegistry.UseCases;
using Athena.Application.BlockManagement.Commands;
using Athena.Application.BlockManagement.Contracts;
using Athena.Application.BlockManagement.Queries;
using Athena.Application.BlockManagement.UseCases;
using Athena.Application.WorkloadRegistry.Commands;
using Athena.Application.WorkloadRegistry.Queries;
using Athena.Application.WorkloadRegistry.UseCases;
using Athena.Application.WorkloadManagement.Commands;
using Athena.Application.WorkloadManagement.Contracts;
using Athena.Application.WorkloadManagement.Queries;
using Athena.Application.WorkloadManagement.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Athena.Application.SystemInstanceManagement.Interfaces;

namespace Athena.Application.Common.Extensions;

public static class AthenaApplicationExtensions
{
    public static IServiceCollection AddAthenaApplication(this IServiceCollection services)
    {
        AddApplicationServices(services);
        AddApplicationExceptionHandling(services);
        AddBackgroundServices(services);

        return services;
    }

    // Application Exception Handling
    private static IServiceCollection AddApplicationExceptionHandling(IServiceCollection services)
    {
        services.AddSingleton<IApplicationExceptionMapper, DomainExceptionMapper>();
        services.AddSingleton<IApplicationExceptionMapper, ApplicationExceptionMapper>();
        services.AddSingleton<ApplicationExceptionService>();
        return services;
    }

    // Background Services
    private static void AddBackgroundServices(IServiceCollection services)
    {
        services.AddScoped<OrchestrationReconciliationService>();
        services.AddScoped<SystemInstanceOperationProcessor>();
        services.AddSingleton<EndpointCheckSchedule>();
        services.AddScoped<SystemSynchronizationService>();
        services.AddScoped<SystemEndpointSynchronizationService>();
    }

    // Application Services
    private static void AddApplicationServices(IServiceCollection services)
    {
        AddRegistryApplicationServices(services);
        AddManagementApplicationServices(services);
    }

    // Registry
    private static void AddRegistryApplicationServices(IServiceCollection services)
    {
        AddSystemRegistryApplicationServices(services);
        AddOrchestrationRegistryApplicationServices(services);
        AddEcosystemRegistryApplicationServices(services);
        AddBlockRegistryApplicationServices(services);
        AddWorkloadRegistryApplicationServices(services);
        AddGroupRegistryApplicationServices(services);
        AddCompanyRegistryApplicationServices(services);
    }

    private static void AddSystemRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<SystemRegistryCommandHandler>();
        services.AddScoped<SystemRegistryQueryHandler>();
        services.AddScoped<SystemRegistryUseCaseHandler>();
    }

    private static void AddOrchestrationRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<OrchestrationRegistryCommandHandler>();
        services.AddScoped<OrchestrationRegistryQueryHandler>();
        services.AddScoped<OrchestrationRegistryUseCaseHandler>();
    }

    private static void AddEcosystemRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<EcosystemRegistryCommandHandler>();
        services.AddScoped<EcosystemRegistryQueryHandler>();
        services.AddScoped<EcosystemRegistryUseCaseHandler>();
    }

    private static void AddBlockRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<BlockRegistryCommandHandler>();
        services.AddScoped<BlockRegistryQueryHandler>();
        services.AddScoped<BlockRegistryUseCaseHandler>();
    }

    private static void AddWorkloadRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<WorkloadRegistryCommandHandler>();
        services.AddScoped<WorkloadRegistryQueryHandler>();
        services.AddScoped<WorkloadRegistryUseCaseHandler>();
    }

    private static void AddGroupRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<GroupRegistryCommandHandler>();
        services.AddScoped<GroupRegistryQueryHandler>();
        services.AddScoped<GroupRegistryUseCaseHandler>();
    }

    private static void AddCompanyRegistryApplicationServices(IServiceCollection services)
    {
        services.AddScoped<CompanyRegistryCommandHandler>();
        services.AddScoped<CompanyRegistryQueryHandler>();
        services.AddScoped<CompanyRegistryUseCaseHandler>();
    }

    // Management
    private static void AddManagementApplicationServices(IServiceCollection services)
    {
        AddSystemInstanceManagementApplicationServices(services);
        AddOrchestrationManagementApplicationServices(services);
        AddEcosystemManagementApplicationServices(services);
        AddBlockManagementApplicationServices(services);
        AddWorkloadManagementApplicationServices(services);
    }

    private static void AddSystemInstanceManagementApplicationServices(IServiceCollection services)
    {
        services.AddScoped<SystemInstanceProcessManagementService>();
        services.AddScoped<SystemInstanceProcessUseCaseHandler>();
        services.AddScoped<ISystemInstanceProcessCoordinator>(sp => sp.GetRequiredService<SystemInstanceProcessManagementService>());
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
        services.AddScoped<OrchestrationSystemInstanceCommandHandler>();
        services.AddScoped<OrchestrationSystemInstanceQueryHandler>();
        services.AddScoped<OrchestrationSystemInstanceUseCaseHandler>();
        services.AddScoped<IOrchestrationSystemInstanceQueries>(sp => sp.GetRequiredService<OrchestrationSystemInstanceUseCaseHandler>());
        services.AddScoped<OrchestrationStatusSynchronizationService>();
        services.AddScoped<IOrchestrationResourceCoordinator, OrchestrationResourceCoordinator>();
        services.AddScoped<IOrchestrationLifecycleQueries>(sp => sp.GetRequiredService<OrchestrationLifecycleQueryHandler>());
        services.AddScoped<IOrchestrationLifecycleCommands>(sp => sp.GetRequiredService<OrchestrationLifecycleUseCaseHandler>());
        services.AddScoped<IOrchestrationLifecycleOperations>(sp => sp.GetRequiredService<OrchestrationLifecycleOperationUseCaseHandler>());
        services.AddScoped<IOrchestrationStatusSynchronizer>(sp => sp.GetRequiredService<OrchestrationStatusSynchronizationService>());
        services.AddScoped<OrchestrationProcessUseCaseHandler>();
    }

    private static void AddEcosystemManagementApplicationServices(IServiceCollection services)
    {
        services.AddScoped<EcosystemManagementUseCaseHandler>();
        services.AddScoped<EcosystemManagementCommandHandler>();
        services.AddScoped<EcosystemManagementQueryHandler>();
        services.AddScoped<IEcosystemManagement>(sp => sp.GetRequiredService<EcosystemManagementUseCaseHandler>());
    }

    private static void AddBlockManagementApplicationServices(IServiceCollection services)
    {
        services.AddScoped<BlockManagementUseCaseHandler>();
        services.AddScoped<BlockManagementCommandHandler>();
        services.AddScoped<BlockManagementQueryHandler>();
        services.AddScoped<IBlockManagement>(sp => sp.GetRequiredService<BlockManagementUseCaseHandler>());
    }

    private static void AddWorkloadManagementApplicationServices(IServiceCollection services)
    {
        services.AddScoped<WorkloadManagementUseCaseHandler>();
        services.AddScoped<WorkloadManagementCommandHandler>();
        services.AddScoped<WorkloadManagementQueryHandler>();
        services.AddScoped<IWorkloadManagement>(sp => sp.GetRequiredService<WorkloadManagementUseCaseHandler>());
    }
}
