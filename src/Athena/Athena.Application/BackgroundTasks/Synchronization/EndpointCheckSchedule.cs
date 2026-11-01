using System.Collections.Concurrent;
using Athena.Application.SystemRegistry.Dtos;

namespace Athena.Application.BackgroundTasks.Synchronization;

public class EndpointCheckSchedule
{
    private record EndpointCheckResult(DateTime LastCheckedAtUtc, bool IsAvailable);
    private readonly ConcurrentDictionary<string, EndpointCheckResult> _results = new();

    public bool ShouldCheck(Guid systemId, string endpointName, string baseAddress, DateTime nowUtc, TimeSpan interval)
    {
        var key = CreateKey(systemId, endpointName, baseAddress);
        return !_results.TryGetValue(key, out var result) || nowUtc - result.LastCheckedAtUtc >= interval;
    }

    public void RecordResult(Guid systemId, string endpointName, string baseAddress, bool isAvailable, DateTime checkedAtUtc)
    {
        var key = CreateKey(systemId, endpointName, baseAddress);
        _results[key] = new EndpointCheckResult(checkedAtUtc, isAvailable);
    }

    public void PruneObsoleteEndpoints(Guid systemId, IReadOnlyList<SystemEndpointDto> endpoints)
    {
        var currentKeys = endpoints
            .Select(endpoint => CreateKey(systemId, endpoint.Name, endpoint.BaseAddress))
            .ToHashSet(StringComparer.Ordinal);
        var systemPrefix = $"{systemId:N}:";
        foreach (var key in _results.Keys)
        {
            if (key.StartsWith(systemPrefix, StringComparison.Ordinal)
                && !currentKeys.Contains(key))
                _results.TryRemove(key, out _);
        }
    }

    public string DetermineAvailability(Guid systemId, IReadOnlyList<SystemEndpointDto> endpoints)
    {
        if (endpoints.Count == 0) return "Unknown";
        var results = endpoints.Select(endpoint =>
        {
            var key = CreateKey(systemId, endpoint.Name, endpoint.BaseAddress);
            return _results.TryGetValue(key, out var result) ? (bool?)result.IsAvailable : null;
        })
        .ToList();
        if (results.Any(result => result == true)) return "Available";
        if (results.All(result => result == false)) return "Unavailable";
        return "Unknown";
    }

    private static string CreateKey(Guid systemId, string endpointName, string baseAddress)
    {
        return $"{systemId:N}:{endpointName}:{baseAddress}";
    }
}