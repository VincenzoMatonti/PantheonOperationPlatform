using Athena.Cli.Application.Interfaces.Systems;
using Athena.Cli.Application.Services.Systems;
using Athena.Cli.Infrastructure.Services.Common;
using Athena.Cli.Infrastructure.Services.Systems;
using Athena.Cli.Presentation.Inputs.Common;
using Athena.Cli.Presentation.Interactives.Common;
using Athena.Cli.Presentation.Interactives.Systems;
using Athena.Cli.Presentation.Outputs.Systems;
using Microsoft.Extensions.DependencyInjection;

namespace Athena.Cli.Infrastructure.Dependencies;

public static class DependencyInjection
{
    public static IServiceCollection AddAthenaCli(this IServiceCollection services, Uri baseAddress)
    {
        services.AddHttpClient<AthenaApiClient>(client => { client.BaseAddress = baseAddress; });
        services.AddTransient<ISystemsRegistry, AthenaSystemsRegistryClient>();
        services.AddTransient<SystemRegistryService>();
        services.AddTransient<CliInput>();
        services.AddTransient<CliInteractive>();
        services.AddTransient<SystemRegistryInteractive>();
        services.AddTransient<SystemRegistryOutput>();
        return services;
    }
}
