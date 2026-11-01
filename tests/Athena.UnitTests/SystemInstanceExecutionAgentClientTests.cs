using Athena.Application.SystemManagement.Contracts;
using Athena.Infrastructure.Services.ProcessManagement;
using Microsoft.Extensions.Configuration;

namespace Athena.UnitTests;

public sealed class SystemInstanceExecutionAgentClientTests
{
    [Fact]
    public async Task OperationWithoutConfiguredBaseUrlFailsExplicitly()
    {
        using var httpClient = new HttpClient();
        var configuration = new ConfigurationBuilder().Build();
        var client = new SystemInstanceExecutionAgentClient(httpClient, configuration);

        await Assert.ThrowsAsync<ExecutionAgentNotConfiguredException>(
            () => client.GetStatusAsync(Guid.NewGuid()));
    }
}
