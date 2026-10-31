using System.Globalization;
using System.Text.Json;
using Athena.Application.Common.Dtos;
using Athena.Application.Common.Interfaces.Runtime;
using Athena.Infrastructure.Services.Client;

namespace Athena.Infrastructure.Services.Runtime.Providers.Docker;

public class DockerRuntimeProvider : IRuntimeProvider
{
    public string ProviderName => "docker";

    public async Task StartAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default)
    {
        Validate(resource);
        await DockerCli.ExecuteAsync(["start", resource.ResourceId], cancellationToken);
    }

    public async Task StopAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default)
    {
        Validate(resource);
        await DockerCli.ExecuteAsync(["stop", resource.ResourceId], cancellationToken);
    }

    public async Task RestartAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default)
    {
        Validate(resource);
        await DockerCli.ExecuteAsync(["restart", resource.ResourceId], cancellationToken);
    }

    public async Task RemoveAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default)
    {
        Validate(resource);
        await DockerCli.ExecuteAsync(["rm", resource.ResourceId], cancellationToken);
    }

    public async Task<RuntimeResourceStatusDto> GetStatusAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default)
    {
        Validate(resource);
        var json = await DockerCli.ExecuteAsync(["inspect", resource.ResourceId], cancellationToken);
        using var document = JsonDocument.Parse(json);
        var container = document.RootElement[0];
        var state = container.GetProperty("State");
        var stateName = state.GetProperty("Status").GetString() ?? "unknown";
        var isRunning = state.GetProperty("Running").GetBoolean();
        var startedAtText = state.GetProperty("StartedAt").GetString();
        DateTime? startedAt = DateTime.TryParse(
            startedAtText, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var parsed) && parsed.Year > 1 ? parsed : null;
        return new RuntimeResourceStatusDto(stateName, isRunning, startedAt, DateTime.UtcNow);
    }

    private static void Validate(RuntimeResourceReferenceDto resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        if (!string.Equals(resource.Provider, "docker", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException($"Il provider '{resource.Provider}' non è Docker.", nameof(resource));
        if (string.IsNullOrWhiteSpace(resource.ResourceId)) throw new ArgumentException("Il resource ID è obbligatorio.", nameof(resource));
    }
}
