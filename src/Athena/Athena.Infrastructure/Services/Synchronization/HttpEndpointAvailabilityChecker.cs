using System.Net;
using Athena.Application.BackgroundTasks.Synchronization.Interfaces;

namespace Athena.Infrastructure.Services.Synchronization;

public class HttpEndpointAvailabilityChecker(HttpClient httpClient) : IEndpointAvailabilityChecker
{
    public async Task<bool> CheckAvailabilityAsync(string baseAddress, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Head, baseAddress);
        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (response.StatusCode is HttpStatusCode.MethodNotAllowed or HttpStatusCode.NotImplemented)
        {
            using var getRequest = new HttpRequestMessage(HttpMethod.Get, baseAddress);
            using var getResponse = await httpClient.SendAsync(getRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            return true;
        }

        return true;
    }
}