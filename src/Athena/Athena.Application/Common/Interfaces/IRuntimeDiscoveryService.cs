using Athena.Application.Common.Dtos;

namespace Athena.Application.Common.Interfaces;

public interface IRuntimeDiscoveryService
{
    Task<IReadOnlyList<SystemRuntimeInstanceDto>> DiscoverAndSynchronizeAsync(CancellationToken cancellationToken = default);
}