using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Systems;

public class AthenaSystemsRuntimeInstancesClient(AthenaApiClient apiClient) : ISystemsRuntimeInstances
{
    private const string BasePath = "api/systems/runtime-instances";

    public Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemRuntimeInstanceResponse>>(BasePath, cancellationToken);

    public async Task<SystemRuntimeInstanceResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await apiClient.GetAsync<SystemRuntimeInstanceResponse>($"{BasePath}/{id}", cancellationToken);
    }

    public Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetBySystemIdAsync(Guid systemId, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemRuntimeInstanceResponse>>($"{BasePath}/by-system/{systemId}", cancellationToken);

    public Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetBySystemIdAndEnvironmentAsync(
        Guid systemId, string environment, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemRuntimeInstanceResponse>>(
            $"{BasePath}/by-system/{systemId}/environment/{Encode(environment)}", cancellationToken);

    public Task<IReadOnlyList<SystemRuntimeInstanceResponse>> GetByRuntimeResourceNameAsync(
        string runtimeResourceName, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemRuntimeInstanceResponse>>($"{BasePath}/by-resource-name/{Encode(runtimeResourceName)}", cancellationToken);

    public Task<SystemRuntimeInstanceResponse> RegisterAsync(RegisterSystemRuntimeInstanceRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<RegisterSystemRuntimeInstanceRequest, SystemRuntimeInstanceResponse>(BasePath, request, cancellationToken);

    public async Task<bool> UnregisterAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await apiClient.DeleteAsync($"{BasePath}/{id}", cancellationToken);
        return true;
    }

    private static string Encode(string value) => Uri.EscapeDataString(value);
}
