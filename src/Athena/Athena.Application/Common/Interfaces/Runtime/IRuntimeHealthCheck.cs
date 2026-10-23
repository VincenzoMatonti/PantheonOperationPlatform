using Athena.Application.Common.Dtos;

namespace Athena.Application.Common.Interfaces.Runtime;

/// <summary>
/// Provides health information about an external runtime resource.
/// </summary>
public interface IRuntimeHealthCheck
{
    string ProviderName { get; }

    Task<RuntimeHealthCheckDto> CheckHealthAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default);
}