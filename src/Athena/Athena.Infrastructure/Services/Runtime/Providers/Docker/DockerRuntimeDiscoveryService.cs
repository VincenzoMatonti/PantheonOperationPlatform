using System.Text.Json;
using Athena.Application.Common.Dtos;
using Athena.Application.Common.Interfaces;
using Athena.Infrastructure.Services.Client;

namespace Athena.Infrastructure.Services.Runtime.Providers.Docker;

public sealed class DockerRuntimeDiscoveryService(ISystemRuntimeInstanceRegistry registry) : IRuntimeDiscoveryService
{
    private const string ManagedLabel = "com.pantheon.managed";
    private const string SystemIdLabel = "com.pantheon.system-id";
    private const string EnvironmentLabel = "com.pantheon.environment";
    private const string RoleLabel = "com.pantheon.role";

    public async Task<IReadOnlyList<SystemRuntimeInstanceDto>> DiscoverAndSynchronizeAsync(CancellationToken cancellationToken = default)
    {
        var output = await DockerCli.ExecuteAsync(["ps", "-aq", "--filter", $"label={ManagedLabel}=true"], cancellationToken);
        var containerIds = output.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Distinct(StringComparer.Ordinal).ToArray();
        if (containerIds.Length == 0) return [];
        var arguments = new List<string> { "inspect" };
        arguments.AddRange(containerIds);
        var json = await DockerCli.ExecuteAsync(arguments, cancellationToken);
        using var document = JsonDocument.Parse(json);
        var synchronized = new List<SystemRuntimeInstanceDto>();
        foreach (var container in document.RootElement.EnumerateArray())
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!TryReadCandidate(container, out var candidate)) continue;
            var instance = await registry.SynchronizeDiscoveredAsync(candidate, cancellationToken);
            if (instance is not null) synchronized.Add(instance);
        }

        return synchronized;
    }

    private static bool TryReadCandidate(JsonElement container, out DiscoveredRuntimeInstanceDto candidate)
    {
        candidate = default!;
        if (!container.TryGetProperty("Config", out var config) || !config.TryGetProperty("Labels", out var labels) || labels.ValueKind != JsonValueKind.Object) return false;
        var managed = GetLabel(labels, ManagedLabel);
        if (!string.Equals(managed, "true", StringComparison.OrdinalIgnoreCase)) return false;
        var role = GetLabel(labels, RoleLabel);
        if (!string.Equals(role, "service", StringComparison.OrdinalIgnoreCase) && !string.Equals(role, "worker", StringComparison.OrdinalIgnoreCase)) return false;
        var systemIdText = GetLabel(labels, SystemIdLabel);
        if (!Guid.TryParse(systemIdText, out var systemId) || systemId == Guid.Empty) return false;
        var environment = GetLabel(labels, EnvironmentLabel);
        if (string.IsNullOrWhiteSpace(environment)) return false;
        if (!container.TryGetProperty("Id", out var idElement)) return false;
        var resourceId = idElement.GetString();
        if (string.IsNullOrWhiteSpace(resourceId)) return false;
        if (!container.TryGetProperty("Name", out var nameElement)) return false;
        var resourceName = nameElement.GetString()?.TrimStart('/');
        if (string.IsNullOrWhiteSpace(resourceName)) return false;
        candidate = new DiscoveredRuntimeInstanceDto(systemId, "docker", resourceId, resourceName, environment.Trim());
        return true;
    }

    private static string? GetLabel(JsonElement labels, string name)
    {
        return labels.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String ? value.GetString() : null;
    }
}