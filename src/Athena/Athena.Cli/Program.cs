using Athena.Cli.Application.Extensions;
using Athena.Cli.Infrastructure.Extensions;
using Athena.Cli.Presentation.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var baseAddress = new Uri(Environment.GetEnvironmentVariable("ATHENA_API_URL") ?? "http://localhost:8080/");
var services = new ServiceCollection();
services.AddAthenaCliApplication();
services.AddAthenaCliInfrastructure(baseAddress);
var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);
app.Configure(config =>
{
    config.SetApplicationName("athena");
    config.AddAthenaCliCommands();
});

return await app.RunAsync(args);

internal class TypeRegistrar(IServiceCollection services) : ITypeRegistrar
{
    public ITypeResolver Build()
    {
        return new TypeResolver(services.BuildServiceProvider());
    }

    public void Register(Type service, Type implementation)
    {
        services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        services.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        services.AddSingleton(service, _ => factory());
    }
}

internal class TypeResolver(IServiceProvider provider) : ITypeResolver, IDisposable
{
    public object? Resolve(Type? type)
    {
        return type is null ? null : provider.GetService(type);
    }

    public void Dispose()
    {
        if (provider is IDisposable disposable) disposable.Dispose();
    }
}
