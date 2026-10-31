using Athena.Cli.Application.Contracts.Systems;

namespace Athena.Cli.Application.Interfaces.Systems;

public interface ISystemsRuntimeInstances
{
    Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SystemRuntimeInstanceResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetBySystemIdAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetBySystemIdAndEnvironmentAsync(Guid systemId, string environment, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetByRuntimeResourceNameAsync(string runtimeResourceName, CancellationToken cancellationToken = default);
    Task<SystemRuntimeInstanceResponse> RegisterAsync(RegisterSystemRuntimeInstanceRequest request, CancellationToken cancellationToken = default);
    Task<bool> UnregisterAsync(Guid id, CancellationToken cancellationToken = default);
}
