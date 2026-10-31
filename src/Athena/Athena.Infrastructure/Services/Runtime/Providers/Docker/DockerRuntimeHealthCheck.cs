using System.Text.Json;
using Athena.Application.Common.Dtos;
using Athena.Application.Common.Interfaces.Runtime;
using Athena.Infrastructure.Services.Client;

namespace Athena.Infrastructure.Services.Runtime.Providers.Docker;

public class DockerRuntimeHealthCheck : IRuntimeHealthCheck
{
    public string ProviderName => "docker";

    public async Task<RuntimeHealthCheckDto> CheckHealthAsync(RuntimeResourceReferenceDto resource, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(resource);
        if (!string.Equals(resource.Provider, ProviderName, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("La risorsa non appartiene al provider Docker.", nameof(resource));
        var json = await DockerCli.ExecuteAsync(["inspect", resource.ResourceId], cancellationToken);
        using var document = JsonDocument.Parse(json);
        var state = document.RootElement[0].GetProperty("State");
        if (!state.TryGetProperty("Health", out var health))
        {
            var running = state.GetProperty("Running").GetBoolean();
            return new RuntimeHealthCheckDto(
                false, running ? "health-check-not-configured" : "not-running", DateTime.UtcNow, "Il container non espone un health check Docker configurato.");
        }

        var status = health.GetProperty("Status").GetString() ?? "unknown";
        return new RuntimeHealthCheckDto(string.Equals(status, "healthy", StringComparison.OrdinalIgnoreCase), status, DateTime.UtcNow);
    }
}
