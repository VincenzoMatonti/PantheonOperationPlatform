using System.Net;
using System.Text.Json;
using Athena.Api.Common.middleware;
using Athena.Cli.Infrastructure.Services.Blocks;
using Athena.Cli.Infrastructure.Services.Common;
using Athena.Cli.Infrastructure.Services.Ecosystems;
using Athena.Cli.Infrastructure.Services.Workloads;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace Athena.UnitTests;

public sealed class AthenaCliHttpContractTests
{
    [Fact]
    public async Task DeleteAsync_WithRequest_SendsJsonBody()
    {
        HttpMethod? capturedMethod = null;
        string? capturedBody = null;
        var handler = new RecordingHandler(async request =>
        {
            capturedMethod = request.Method;
            capturedBody = await request.Content!.ReadAsStringAsync();
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("{}")
            };
        });
        using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };
        var apiClient = new AthenaApiClient(httpClient);

        await Assert.ThrowsAsync<AthenaApiException>(() =>
            apiClient.DeleteAsync<DeletePayload, object>(
                "api/systems/00000000-0000-0000-0000-000000000001/endpoints",
                new DeletePayload("health")));

        Assert.Equal(HttpMethod.Delete, capturedMethod);
        using var payload = JsonDocument.Parse(capturedBody!);
        Assert.Equal("health", payload.RootElement.GetProperty("name").GetString());
    }

    [Theory]
    [InlineData("block")]
    [InlineData("workload")]
    [InlineData("ecosystem")]
    public async Task LifecycleGet_ReturnsNullForNotFound(string resource)
    {
        using var httpClient = new HttpClient(new RecordingHandler(_ =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound))))
        {
            BaseAddress = new Uri("http://localhost/")
        };
        var apiClient = new AthenaApiClient(httpClient);
        var id = Guid.NewGuid();

        object? result = resource switch
        {
            "block" => await new AthenaBlockManagementClient(apiClient).GetAsync(id),
            "workload" => await new AthenaWorkloadManagementClient(apiClient).GetAsync(id),
            "ecosystem" => await new AthenaEcosystemManagementClient(apiClient).GetAsync(id),
            _ => throw new ArgumentOutOfRangeException(nameof(resource))
        };

        Assert.Null(result);
    }

    [Fact]
    public async Task LifecycleGet_PropagatesNonNotFoundErrors()
    {
        using var httpClient = new HttpClient(new RecordingHandler(_ =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable))))
        {
            BaseAddress = new Uri("http://localhost/")
        };
        var apiClient = new AthenaApiClient(httpClient);

        await Assert.ThrowsAsync<AthenaApiException>(() =>
            new AthenaBlockManagementClient(apiClient).GetAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GlobalExceptionHandler_DoesNotWriteErrorForAbortedRequest()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestAborted = new CancellationToken(canceled: true)
        };
        var handler = new GlobalExceptionHandler(
            null!,
            NullLogger<GlobalExceptionHandler>.Instance);

        var handled = await handler.TryHandleAsync(
            httpContext,
            new OperationCanceledException(),
            httpContext.RequestAborted);

        Assert.True(handled);
        Assert.Equal(StatusCodes.Status200OK, httpContext.Response.StatusCode);
        Assert.False(httpContext.Response.HasStarted);
    }

    private sealed record DeletePayload(string Name);

    private sealed class RecordingHandler(
        Func<HttpRequestMessage, Task<HttpResponseMessage>> send) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken) => send(request);
    }
}
