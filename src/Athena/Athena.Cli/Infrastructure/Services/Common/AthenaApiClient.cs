using System.Net.Http.Json;
using System.Text.Json;
using Athena.Cli.Application.Contracts.Api;

namespace Athena.Cli.Infrastructure.Services.Common;

public class AthenaApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<T> GetAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        return SendForDataAsync<T>(HttpMethod.Get, path, null, false, cancellationToken);
    }

    public Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken = default)
    {
        return SendForDataAsync<TResponse>(HttpMethod.Post, path, request, true, cancellationToken);
    }

    public Task<TResponse> PostAsync<TResponse>(string path, CancellationToken cancellationToken = default)
    {
        return SendForDataAsync<TResponse>(HttpMethod.Post, path, null, false, cancellationToken);
    }

    public Task<TResponse> PatchAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken = default)
    {
        return SendForDataAsync<TResponse>(HttpMethod.Patch, path, request, true, cancellationToken);
    }

    public Task<TResponse> DeleteAsync<TResponse>(string path, CancellationToken cancellationToken = default)
    {
        return SendForDataAsync<TResponse>(HttpMethod.Delete, path, null, false, cancellationToken);
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.DeleteAsync(path, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private async Task<T> SendForDataAsync<T>(HttpMethod method, string path, object? requestBody, bool includeBody, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, path);
        if (includeBody) request.Content = JsonContent.Create(requestBody, options: JsonOptions);
        using var response = await httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions, cancellationToken);
        if (!response.IsSuccessStatusCode || body is null || !body.Success)
        {
            var details = body is null
                ? "Athena API returned an empty or invalid response."
                : body.Errors.Count == 0
                    ? "No error details were provided."
                    : string.Join(Environment.NewLine, body.Errors.Select(error => $"{error.Code}: {error.Message}"));
            throw new AthenaApiException((int)response.StatusCode, details);
        }

        return body.Data ?? throw new InvalidOperationException($"Athena API returned a successful response without data. Route: {path}");
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode) return;
        var body = await response.Content.ReadFromJsonAsync<ApiResponse>(JsonOptions, cancellationToken);
        var details = body is null || body.Errors.Count == 0
            ? $"Athena API returned HTTP {(int)response.StatusCode}."
            : string.Join(Environment.NewLine, body.Errors.Select(error => $"{error.Code}: {error.Message}"));
        throw new AthenaApiException((int)response.StatusCode, details);
    }
}

public class AthenaApiException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
