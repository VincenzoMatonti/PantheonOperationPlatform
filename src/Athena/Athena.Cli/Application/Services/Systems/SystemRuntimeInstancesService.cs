using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;

namespace Athena.Cli.Application.Services.Systems;

public class SystemRuntimeInstancesService(
    ISystemsRuntimeInstances runtimeInstances)
{
    public Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        runtimeInstances.GetAllAsync(cancellationToken);

    public Task<SystemRuntimeInstanceResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        runtimeInstances.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetBySystemIdAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        runtimeInstances.GetBySystemIdAsync(systemId, cancellationToken);

    public Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetBySystemIdAndEnvironmentAsync(
        Guid systemId, string environment, CancellationToken cancellationToken = default) =>
           runtimeInstances.GetBySystemIdAndEnvironmentAsync(systemId, environment, cancellationToken);

    public Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetByRuntimeResourceNameAsync(
        string runtimeResourceName, CancellationToken cancellationToken = default) =>
            runtimeInstances.GetByRuntimeResourceNameAsync(runtimeResourceName, cancellationToken);

    public Task<SystemRuntimeInstanceResponse> RegisterAsync(RegisterSystemRuntimeInstanceRequest request, CancellationToken cancellationToken = default) =>
        runtimeInstances.RegisterAsync(request, cancellationToken);

    public Task<bool> UnregisterAsync(Guid id, CancellationToken cancellationToken = default) =>
        runtimeInstances.UnregisterAsync(id, cancellationToken);
}
