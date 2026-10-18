using Athena.Api.Common.builders;
using Athena.Api.Common.interfaces;
using Athena.Api.Common.mappers;
using Athena.Api.Common.resolvers;
using Athena.Api.Common.services;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Api.Common.extensions;

public static class ApiCommonServiceCollectionExtensions
{
    public static IServiceCollection AddAthenaApiCommonServices(this IServiceCollection services)
    {
        services.AddSingleton<ExceptionMapperResolver>();
        services.AddSingleton<ExceptionResponseBuilder>();
        services.AddSingleton<ExceptionHandlingService>();
        services.AddSingleton<IExceptionMapper, ApiApplicationCoreExceptionMapper>();
        services.AddSingleton<IExceptionMapper, ApiCommonExceptionMapper>();
        services.AddSingleton<IExceptionMapper, ApiValidationExceptionMapper>();

        return services;
    }
}