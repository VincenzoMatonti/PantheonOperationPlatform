using System.Runtime.CompilerServices;
using Athena.Application.Common.Dtos;
using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Interfaces.Runtime;

namespace Athena.Application.Common.Services;

/// <summary>
/// Coordinates application-level operations on registered runtime instances.
/// Does not depend on concrete runtime providers or persistence models.
/// </summary>
public class RuntimeApplicationService(
    ISystemRuntimeInstanceRegistry registry,
    IRuntimeProviderResolver providerResolver,
    IRuntimeDiscoveryService runtimeDiscoveryService)
{
    public Task<SystemRuntimeInstanceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => registry.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        registry.GetAllAsync(cancellationToken);

    public Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetByRuntimeResourceNameAsync(string runtimeResourceName, CancellationToken cancellationToken = default) =>
        registry.GetByRuntimeResourceNameAsync(runtimeResourceName, cancellationToken);

    public Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetBySystemIdAsync(
        Guid systemId, CancellationToken cancellationToken = default) => registry.GetBySystemIdAsync(systemId, cancellationToken);

    public Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetBySystemIdAndEnvironmentAsync(
        Guid systemId, string environment, CancellationToken cancellationToken = default) =>
        registry.GetBySystemIdAndEnvironmentAsync(systemId, environment, cancellationToken);
    public Task<SystemRuntimeInstanceDto> RegisterAsync(
        Guid systemId, string runtimeProvider, string runtimeResourceId, string runtimeResourceName, string environment, CancellationToken cancellationToken = default)
        => registry.RegisterAsync(systemId, runtimeProvider, runtimeResourceId, runtimeResourceName, environment, cancellationToken);

    public Task<bool> UnregisterAsync(Guid id, CancellationToken cancellationToken = default) => registry.UnregisterAsync(id, cancellationToken);

    public Task StartAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default) =>
        ExecuteProviderOperationAsync(runtimeInstanceId, (provider, resource, token) => provider.StartAsync(resource, token), cancellationToken);

    public Task StopAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default) =>
        ExecuteProviderOperationAsync(runtimeInstanceId, (provider, resource, token) => provider.StopAsync(resource, token), cancellationToken);

    public Task RestartAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default) =>
        ExecuteProviderOperationAsync(runtimeInstanceId, (provider, resource, token) => provider.RestartAsync(resource, token), cancellationToken);

    public Task RemoveResourceAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default) =>
        ExecuteProviderOperationAsync(runtimeInstanceId, (provider, resource, token) => provider.RemoveAsync(resource, token), cancellationToken);

    public async Task<RuntimeResourceStatusDto> GetStatusAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await GetDiagnosticInstanceAsync(runtimeInstanceId, cancellationToken);
        var resource = ToResourceReference(instance);
        var provider = providerResolver.Resolve(instance.RuntimeProvider);
        return await provider.GetStatusAsync(resource, cancellationToken);
    }

    public async Task<RuntimeHealthCheckDto> CheckHealthAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default)
    {
        var instance = await GetDiagnosticInstanceAsync(runtimeInstanceId, cancellationToken);
        var resource = ToResourceReference(instance);
        var healthCheck = providerResolver.ResolveHealthCheck(instance.RuntimeProvider);
        return await healthCheck.CheckHealthAsync(resource, cancellationToken);
    }

    public Task<IReadOnlyList<RuntimeInstanceDiagnosticsDto>> DiscoverWithDiagnosticsAsync(CancellationToken cancellationToken = default)
    {
        return runtimeDiscoveryService.DiscoverWithDiagnosticsAsync(cancellationToken);
    }

    public async IAsyncEnumerable<RuntimeLogEntryDto> ReadLogsAsync(
        Guid runtimeInstanceId, RuntimeLogOptionsDto options, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        var instance = await GetDiagnosticInstanceAsync(runtimeInstanceId, cancellationToken);
        var resource = ToResourceReference(instance);
        var logReader = providerResolver.ResolveLogReader(instance.RuntimeProvider);
        await foreach (var entry in logReader.ReadLogsAsync(resource, options, cancellationToken).WithCancellation(cancellationToken))
        {
            yield return entry;
        }
    }

    private async Task<SystemRuntimeInstanceDto> GetDiagnosticInstanceAsync(Guid runtimeInstanceId, CancellationToken cancellationToken)
    {
        var instance = await GetRequiredInstanceAsync(runtimeInstanceId, cancellationToken);
        if (!string.Equals(instance.RuntimeProvider, "docker", StringComparison.OrdinalIgnoreCase)) return instance;
        await runtimeDiscoveryService.DiscoverAndSynchronizeAsync(cancellationToken);
        return await registry.GetByIdAsync(runtimeInstanceId, cancellationToken)
            ?? throw new InvalidOperationException($"Runtime instance '{runtimeInstanceId}' non trovata dopo la sincronizzazione.");
    }

    private async Task ExecuteProviderOperationAsync(
        Guid runtimeInstanceId, Func<IRuntimeProvider, RuntimeResourceReferenceDto, CancellationToken, Task> operation, CancellationToken cancellationToken)
    {
        var instance = await GetRequiredInstanceAsync(runtimeInstanceId, cancellationToken);
        var resource = ToResourceReference(instance);
        var provider = providerResolver.Resolve(instance.RuntimeProvider);
        await operation(provider, resource, cancellationToken);
    }

    private async Task<SystemRuntimeInstanceDto> GetRequiredInstanceAsync(Guid runtimeInstanceId, CancellationToken cancellationToken)
    {
        if (runtimeInstanceId == Guid.Empty) throw new ArgumentException("Il runtime instance ID non può essere vuoto.", nameof(runtimeInstanceId));
        return await registry.GetByIdAsync(runtimeInstanceId, cancellationToken) ??
               throw new InvalidOperationException($"Runtime instance '{runtimeInstanceId}' non trovata.");
    }

    private static RuntimeResourceReferenceDto ToResourceReference(SystemRuntimeInstanceDto instance) =>
        new(instance.RuntimeProvider, instance.RuntimeResourceId, instance.Environment);
}
