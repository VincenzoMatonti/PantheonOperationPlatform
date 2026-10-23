using Athena.Application.Common.Dtos;

namespace Athena.Application.Common.Interfaces;

/// <summary>
/// Application contract for registering and retrieving runtime instances
/// associated with registered Pantheon systems.
/// </summary>
public interface ISystemRuntimeInstanceRegistry
{
    Task<SystemRuntimeInstanceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetBySystemIdAsync(Guid systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetBySystemIdAndEnvironmentAsync(Guid systemId, string environment, CancellationToken cancellationToken = default);
    Task<SystemRuntimeInstanceDto> RegisterAsync(
        Guid systemId,
        string runtimeProvider,
        string runtimeResourceId,
        string runtimeResourceName,
        string environment,
        CancellationToken cancellationToken = default);

    Task<bool> UnregisterAsync(Guid id, CancellationToken cancellationToken = default);
}