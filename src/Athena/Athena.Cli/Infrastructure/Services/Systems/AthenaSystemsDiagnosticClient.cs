using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Systems;

public class AthenaSystemsDiagnosticClient(AthenaApiClient apiClient) : ISystemsDiagnostic
{
    private const string BasePath = "api/systems/diagnostics/runtime-instances";

    public Task<RuntimeResourceStatusResponse> GetRuntimeStatusAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<RuntimeResourceStatusResponse>($"{BasePath}/{runtimeInstanceId}/status", cancellationToken);

    public Task<RuntimeHealthCheckResponse> CheckRuntimeHealthAsync(Guid runtimeInstanceId, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<RuntimeHealthCheckResponse>($"{BasePath}/{runtimeInstanceId}/health", cancellationToken);

    public Task<IReadOnlyList<RuntimeLogEntryResponse>> ReadRuntimeLogsAsync(
        Guid runtimeInstanceId, int tail = 100, DateTime? sinceUtc = null, DateTime? untilUtc = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = new List<string> { $"tail={tail}" };
        if (sinceUtc.HasValue) queryParameters.Add($"sinceUtc={Encode(sinceUtc.Value.ToString("O"))}");
        if (untilUtc.HasValue) queryParameters.Add($"untilUtc={Encode(untilUtc.Value.ToString("O"))}");
        var queryString = string.Join("&", queryParameters);
        return apiClient.GetAsync<IReadOnlyList<RuntimeLogEntryResponse>>($"{BasePath}/{runtimeInstanceId}/logs?{queryString}", cancellationToken);
    }

    private static string Encode(string value) => Uri.EscapeDataString(value);
}
