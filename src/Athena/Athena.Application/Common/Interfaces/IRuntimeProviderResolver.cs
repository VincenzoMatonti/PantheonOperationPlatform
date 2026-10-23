using Athena.Application.Common.Interfaces.Runtime;

namespace Athena.Application.Common.Interfaces;

/// <summary>
/// Resolves runtime providers and their supported capabilities.
/// </summary>
public interface IRuntimeProviderResolver
{
    IRuntimeProvider Resolve(string providerName);

    IRuntimeHealthCheck ResolveHealthCheck(string providerName);

    IRuntimeLogReader ResolveLogReader(string providerName);
}