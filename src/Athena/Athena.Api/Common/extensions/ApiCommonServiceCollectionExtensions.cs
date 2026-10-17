using Athena.Api.Common.builders;
using Athena.Api.Common.interfaces;
using Athena.Api.Common.mappers;
using Athena.Api.Common.mappers.application;
using Athena.Api.Common.mappers.domain;
using Athena.Api.Common.resolvers;
using Athena.Api.Common.services;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Api.Common.extensions;

public static class ApiCommonServiceCollectionExtensions
{
    public static IServiceCollection AddAthenaApiCommonServices(this IServiceCollection services)
    {
        // Exception handling
        services.AddSingleton<ExceptionMapperResolver>();
        services.AddSingleton<ExceptionResponseBuilder>();
        services.AddSingleton<ExceptionHandlingService>();

        // Domain exception mappers
        services.AddSingleton<IExceptionMapper, SystemRegistryDomainExceptionMapper>();
        services.AddSingleton<IExceptionMapper, SystemLifecycleDomainExceptionMapper>();
        services.AddSingleton<IExceptionMapper, SystemLifecycleOperationDomainExceptionMapper>();
        services.AddSingleton<IExceptionMapper, OrchestrationDomainExceptionMapper>();
        services.AddSingleton<IExceptionMapper, OrchestrationLifecycleDomainExceptionMapper>();
        services.AddSingleton<IExceptionMapper, OrchestrationLifecycleOperationDomainExceptionMapper>();

        // Application exception mappers
        // Aggiungere qui tutti i mapper applicativi già presenti nel progetto.
        // Esempio:
        services.AddSingleton<IExceptionMapper, SystemRegistryApplicationExceptionMapper>();

        // API common exception mappers
        services.AddSingleton<IExceptionMapper, ApiCommonExceptionMapper>();

        return services;
    }
}
