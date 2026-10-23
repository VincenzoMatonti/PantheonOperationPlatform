using Athena.Application.Common.Dtos;

namespace Athena.Application.Common.Interfaces.Runtime;

/// <summary>
/// Defines the runtime resource operations required by Athena.
/// Implementations belong to Infrastructure.
/// </summary>
public interface IRuntimeProvider
{
    /// <summary>
    /// Stable provider identifier, for example "docker".
    /// </summary>
    string ProviderName { get; }

    Task StartAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default);
    Task StopAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default);
    Task RestartAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes the external runtime resource.
    /// This does not remove the Athena registry entry automatically.
    /// </summary>
    Task RemoveAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default);
    Task<RuntimeResourceStatusDto> GetStatusAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default);
}