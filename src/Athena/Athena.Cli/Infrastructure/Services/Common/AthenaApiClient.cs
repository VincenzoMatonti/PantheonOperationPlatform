using System.Net.Http.Json;
using System.Text.Json;
using Athena.Cli.Application.Contracts.Api;

namespace Athena.Cli.Infrastructure.Services.Common;

public class AthenaApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public Task<T> GetAsync<T>(string path, CancellationToken cancellationToken = default) => SendForDataAsync<T>(HttpMethod.Get, path, null, false, cancellationToken);

    public Task<TResponse> PostAsync<TRequest, TResponse>(
        string path, TRequest request, CancellationToken cancellationToken = default) => SendForDataAsync<TResponse>(HttpMethod.Post, path, request, true, cancellationToken);

    public Task<TResponse> PostAsync<TResponse>(string path, CancellationToken cancellationToken = default) => SendForDataAsync<TResponse>(HttpMethod.Post, path, null, false, cancellationToken);

    public Task<TResponse> PatchAsync<TRequest, TResponse>(
        string path, TRequest request, CancellationToken cancellationToken = default) => SendForDataAsync<TResponse>(HttpMethod.Patch, path, request, true, cancellationToken);

    public Task<TResponse> DeleteAsync<TResponse>(string path, CancellationToken cancellationToken = default) => SendForDataAsync<TResponse>(HttpMethod.Delete, path, null, false, cancellationToken);

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
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode) throw new AthenaApiException((int)response.StatusCode, ExtractErrorDetails(content, (int)response.StatusCode));
        ApiResponse<T>? body;
        try
        {
            body = JsonSerializer.Deserialize<ApiResponse<T>>(content, JsonOptions);
        }
        catch (JsonException exception)
        {
            throw new AthenaApiException((int)response.StatusCode, $"Invalid Athena API response for '{path}': {exception.Message}. " + $"Response body: {content}");
        }

        if (body is null) throw new AthenaApiException((int)response.StatusCode, $"Athena API returned an empty response for '{path}'.");
        if (!body.Success)
        {
            var details = body.Errors.Count == 0 ? "No error details were provided." : string.Join(Environment.NewLine, body.Errors.Select(error => $"{error.Code}: {error.Message}"));
            throw new AthenaApiException((int)response.StatusCode, details);
        }

        return body.Data ?? throw new AthenaApiException((int)response.StatusCode, $"Athena API returned a successful response without data. Route: {path}");
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode) return;
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new AthenaApiException((int)response.StatusCode, ExtractErrorDetails(content, (int)response.StatusCode));
    }

    private static string ExtractErrorDetails(string content, int statusCode)
    {
        if (string.IsNullOrWhiteSpace(content)) return $"Athena API returned HTTP {statusCode} with an empty response.";
        try
        {
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;
            if (root.TryGetProperty("errors", out var errors))
            {
                if (errors.ValueKind == JsonValueKind.Array)
                {
                    var messages = new List<string>();
                    foreach (var error in errors.EnumerateArray())
                    {
                        var code = error.TryGetProperty("code", out var codeElement) ? codeElement.ToString() : null;
                        var message = error.TryGetProperty("message", out var messageElement) ? messageElement.ToString() : null;
                        messages.Add(string.IsNullOrWhiteSpace(code) ? message ?? error.ToString() : $"{code}: {message}");
                    }

                    if (messages.Count > 0) return string.Join(Environment.NewLine, messages);
                }
                else if (errors.ValueKind == JsonValueKind.Object)
                {
                    return $"Athena API returned HTTP {statusCode}: {errors}";
                }
            }

            foreach (var property in new[] { "detail", "title", "message" })
            {
                if (root.TryGetProperty(property, out var value) && value.ValueKind == JsonValueKind.String) return $"Athena API returned HTTP {statusCode}: {value.GetString()}";
            }
        }
        catch (JsonException)
        {
            // Preserve the original body when it isn't valid JSON.
        }

        return $"Athena API returned HTTP {statusCode}. Response body: {content}";
    }
}

public class AthenaApiException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}
