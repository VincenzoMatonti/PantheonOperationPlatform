using System.Net.Http.Json;
using Athena.Application.SystemManagement.Contracts;
using Microsoft.Extensions.Configuration;

namespace Athena.Infrastructure.Services.ProcessManagement;

public sealed class SystemInstanceExecutionAgentClient : ISystemInstanceExecutionAgent
{
    private readonly HttpClient _httpClient;

    public SystemInstanceExecutionAgentClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        if (_httpClient.BaseAddress is null)
        {
            var value = ResolveBaseUrl(configuration);
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (!Uri.TryCreate(value, UriKind.Absolute, out var baseAddress) ||
                    (baseAddress.Scheme != Uri.UriSchemeHttp && baseAddress.Scheme != Uri.UriSchemeHttps))
                {
                    throw new InvalidOperationException("Athena:ExecutionAgent:BaseUrl must be an absolute HTTP or HTTPS URL.");
                }

                _httpClient.BaseAddress = new Uri(value.TrimEnd('/') + "/", UriKind.Absolute);
            }
        }
    }

    public Task<SystemInstanceExecutionStatus> CreateAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<SystemInstanceExecutionStatus>(HttpMethod.Post, "system-instances", request, cancellationToken);

    public Task<SystemInstanceExecutionStatus> RegisterAsync(SystemInstanceExecutionCommandRequest request, CancellationToken cancellationToken = default) =>
        SendAsync<SystemInstanceExecutionStatus>(HttpMethod.Post, "system-instances/register", request, cancellationToken);

    public Task<SystemInstanceExecutionStatus> StartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
        SendAsync<SystemInstanceExecutionStatus>(HttpMethod.Post, $"system-instances/{systemInstanceId}/start", cancellationToken: cancellationToken);

    public Task<SystemInstanceExecutionStatus> StopAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
        SendAsync<SystemInstanceExecutionStatus>(HttpMethod.Post, $"system-instances/{systemInstanceId}/stop", cancellationToken: cancellationToken);

    public Task<SystemInstanceExecutionStatus> RestartAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
        SendAsync<SystemInstanceExecutionStatus>(HttpMethod.Post, $"system-instances/{systemInstanceId}/restart", cancellationToken: cancellationToken);

    public Task<SystemInstanceExecutionStatus> RemoveAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
        SendAsync<SystemInstanceExecutionStatus>(HttpMethod.Delete, $"system-instances/{systemInstanceId}", cancellationToken: cancellationToken);

    public Task<SystemInstanceExecutionStatus> GetStatusAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
        SendAsync<SystemInstanceExecutionStatus>(HttpMethod.Get, $"system-instances/{systemInstanceId}/status", cancellationToken: cancellationToken);

    public Task<SystemInstanceExecutionHealth> GetHealthAsync(Guid systemInstanceId, CancellationToken cancellationToken = default) =>
        SendAsync<SystemInstanceExecutionHealth>(HttpMethod.Get, $"system-instances/{systemInstanceId}/health", cancellationToken: cancellationToken);

    public async Task<IReadOnlyList<SystemInstanceExecutionLogEntry>> GetLogsAsync(Guid systemInstanceId, int tail = 100, CancellationToken cancellationToken = default)
    {
        var response = await SendAsync<List<SystemInstanceExecutionLogEntry>>(HttpMethod.Get, $"system-instances/{systemInstanceId}/logs?tail={tail}", cancellationToken: cancellationToken);
        return response ?? throw new InvalidOperationException("The execution agent returned an empty log response.");
    }

    private async Task<T> SendAsync<T>(HttpMethod method, string relativePath, object? payload = null, CancellationToken cancellationToken = default)
    {
        if (_httpClient.BaseAddress is null)
            throw new ExecutionAgentNotConfiguredException();

        using var request = new HttpRequestMessage(method, relativePath);
        if (payload is not null)
        {
            request.Content = JsonContent.Create(payload);
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"The execution agent rejected the request '{relativePath}' with status {(int)response.StatusCode}: {body}",
                null,
                response.StatusCode);
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
                ?? throw new ExecutionAgentResponseException($"The execution agent returned an empty response for '{relativePath}'.");
        }
        catch (System.Text.Json.JsonException exception)
        {
            throw new ExecutionAgentResponseException($"The execution agent returned invalid JSON for '{relativePath}': {exception.Message}");
        }
    }

    private static string? ResolveBaseUrl(IConfiguration configuration)
    {
        var value = configuration["Athena:ExecutionAgent:BaseUrl"] ?? configuration["Athena__ExecutionAgent__BaseUrl"];
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
