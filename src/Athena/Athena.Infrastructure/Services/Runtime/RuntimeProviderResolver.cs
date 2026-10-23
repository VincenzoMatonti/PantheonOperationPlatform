using Athena.Application.Common.Interfaces;
using Athena.Application.Common.Interfaces.Runtime;

namespace Athena.Infrastructure.Services.Runtime;

public sealed class RuntimeProviderResolver(
    IEnumerable<IRuntimeProvider> providers,
    IEnumerable<IRuntimeHealthCheck> healthChecks,
    IEnumerable<IRuntimeLogReader> logReaders) : IRuntimeProviderResolver
{
    private readonly IReadOnlyDictionary<string, IRuntimeProvider> _providers = CreateLookup(providers, x => x.ProviderName);
    private readonly IReadOnlyDictionary<string, IRuntimeHealthCheck> _healthChecks = CreateLookup(healthChecks, x => x.ProviderName);
    private readonly IReadOnlyDictionary<string, IRuntimeLogReader> _logReaders = CreateLookup(logReaders, x => x.ProviderName);
    public IRuntimeProvider Resolve(string providerName) => ResolveFrom(_providers, providerName, "runtime provider");
    public IRuntimeHealthCheck ResolveHealthCheck(string providerName) => ResolveFrom(_healthChecks, providerName, "runtime health check");
    public IRuntimeLogReader ResolveLogReader(string providerName) => ResolveFrom(_logReaders, providerName, "runtime log reader");

    private static IReadOnlyDictionary<string, T> CreateLookup<T>(IEnumerable<T> services, Func<T, string> getName)
    {
        var result = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        foreach (var service in services)
        {
            var name = getName(service);
            if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException($"Il servizio {typeof(T).Name} dichiara un nome vuoto.");
            if (!result.TryAdd(name, service)) throw new InvalidOperationException($"Esiste più di un servizio {typeof(T).Name} " + $"per il provider '{name}'.");
        }

        return result;
    }

    private static T ResolveFrom<T>(IReadOnlyDictionary<string, T> lookup, string providerName, string capability)
    {
        if (string.IsNullOrWhiteSpace(providerName)) throw new ArgumentException("Il nome del provider è obbligatorio.", nameof(providerName));
        if (lookup.TryGetValue(providerName.Trim(), out var service)) return service;
        throw new NotSupportedException($"Il provider '{providerName}' non supporta la capacità richiesta: " + $"{capability}.");
    }
}
