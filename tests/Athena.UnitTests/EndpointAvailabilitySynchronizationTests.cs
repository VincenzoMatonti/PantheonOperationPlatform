using System.Net;
using Athena.Application.BackgroundTasks.Synchronization;
using Athena.Application.SystemRegistry.Dtos;
using Athena.Infrastructure.Services.Synchronization;

namespace Athena.UnitTests;

public sealed class EndpointAvailabilitySynchronizationTests
{
    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.NoContent, true)]
    [InlineData(HttpStatusCode.BadRequest, false)]
    [InlineData(HttpStatusCode.InternalServerError, false)]
    public async Task CheckAvailabilityAsync_UsesSuccessStatusCode(
        HttpStatusCode statusCode,
        bool expectedAvailable)
    {
        using var client = CreateClient((_, _) => Task.FromResult(new HttpResponseMessage(statusCode)));
        var checker = new HttpEndpointAvailabilityChecker(client);

        var result = await checker.CheckAvailabilityAsync("https://service.test/health");

        Assert.Equal(expectedAvailable, result);
    }

    [Theory]
    [InlineData(HttpStatusCode.OK, true)]
    [InlineData(HttpStatusCode.ServiceUnavailable, false)]
    public async Task CheckAvailabilityAsync_FallbackGetStatusIsEvaluated(
        HttpStatusCode getStatusCode,
        bool expectedAvailable)
    {
        var methods = new List<HttpMethod>();
        using var client = CreateClient((request, _) =>
        {
            methods.Add(request.Method);
            return Task.FromResult(new HttpResponseMessage(
                request.Method == HttpMethod.Head ? HttpStatusCode.MethodNotAllowed : getStatusCode));
        });
        var checker = new HttpEndpointAvailabilityChecker(client);

        var result = await checker.CheckAvailabilityAsync("https://service.test/health");

        Assert.Equal(expectedAvailable, result);
        Assert.Equal([HttpMethod.Head, HttpMethod.Get], methods);
    }

    [Fact]
    public async Task CheckAvailabilityAsync_PropagatesTimeoutAndConnectionErrors()
    {
        using var timeoutClient = CreateClient((_, _) => Task.FromException<HttpResponseMessage>(new TaskCanceledException("timeout")));
        var timeoutChecker = new HttpEndpointAvailabilityChecker(timeoutClient);

        await Assert.ThrowsAsync<TaskCanceledException>(
            () => timeoutChecker.CheckAvailabilityAsync("https://service.test/health"));

        using var connectionClient = CreateClient((_, _) => Task.FromException<HttpResponseMessage>(new HttpRequestException("DNS failure")));
        var connectionChecker = new HttpEndpointAvailabilityChecker(connectionClient);

        await Assert.ThrowsAsync<HttpRequestException>(
            () => connectionChecker.CheckAvailabilityAsync("https://service.test/health"));
    }

    [Fact]
    public void EndpointCheckSchedule_CachesResultsForFiveMinuteIntervalAndPrunesRemovedEndpoints()
    {
        var schedule = new EndpointCheckSchedule();
        var systemId = Guid.NewGuid();
        var start = DateTime.UtcNow;
        var endpoint = Endpoint("https://service.test/health");
        schedule.RecordResult(systemId, endpoint.Name, endpoint.BaseAddress, true, start);

        Assert.False(schedule.ShouldCheck(systemId, endpoint.Name, endpoint.BaseAddress, start.AddMinutes(4), TimeSpan.FromMinutes(5)));
        Assert.Equal("Available", schedule.DetermineAvailability(systemId, [endpoint]));
        Assert.True(schedule.ShouldCheck(systemId, endpoint.Name, endpoint.BaseAddress, start.AddMinutes(5), TimeSpan.FromMinutes(5)));

        schedule.RecordResult(systemId, endpoint.Name, endpoint.BaseAddress, false, start.AddMinutes(5));
        Assert.Equal("Unavailable", schedule.DetermineAvailability(systemId, [endpoint]));
        Assert.False(schedule.ShouldCheck(systemId, endpoint.Name, endpoint.BaseAddress, start.AddMinutes(6), TimeSpan.FromMinutes(5)));

        schedule.PruneObsoleteEndpoints(systemId, []);

        Assert.True(schedule.ShouldCheck(systemId, endpoint.Name, endpoint.BaseAddress, start.AddMinutes(6), TimeSpan.FromMinutes(5)));
    }

    private static HttpClient CreateClient(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync) =>
        new(new StubHttpMessageHandler(sendAsync));

    private static SystemEndpointDto Endpoint(string address) =>
        new() { Name = "health", BaseAddress = address };

    private sealed class StubHttpMessageHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            sendAsync(request, cancellationToken);
    }
}
