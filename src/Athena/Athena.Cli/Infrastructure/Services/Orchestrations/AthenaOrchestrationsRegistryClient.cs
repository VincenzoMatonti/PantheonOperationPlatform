using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Interfaces.Orchestrations;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Orchestrations;

public class AthenaOrchestrationsRegistryClient(AthenaApiClient apiClient) : IOrchestrationsRegistry
{
    private const string BasePath = "api/orchestrations";

    // Commands
    public Task<OrchestrationResponse> CreateAsync(CreateOrchestrationRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<CreateOrchestrationRequest, OrchestrationResponse>(BasePath, request, cancellationToken);

    public Task<OrchestrationResponse> RegisterAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationResponse>($"{BasePath}/{id}/register", cancellationToken);

    public Task<OrchestrationResponse> UnregisterAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationResponse>($"{BasePath}/{id}/unregister", cancellationToken);

    public Task<OrchestrationResponse> EnableAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationResponse>($"{BasePath}/{id}/enable", cancellationToken);

    public Task<OrchestrationResponse> DisableAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationResponse>($"{BasePath}/{id}/disable", cancellationToken);

    public Task<OrchestrationResponse> RenameAsync(Guid id, RenameOrchestrationRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PatchAsync<RenameOrchestrationRequest, OrchestrationResponse>($"{BasePath}/{id}/name", request, cancellationToken);

    public Task<OrchestrationResponse> MarkAvailableAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationResponse>($"{BasePath}/{id}/availability/available", cancellationToken);

    public Task<OrchestrationResponse> MarkUnavailableAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationResponse>($"{BasePath}/{id}/availability/unavailable", cancellationToken);

    public Task<OrchestrationResponse> MarkAvailabilityUnknownAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationResponse>($"{BasePath}/{id}/availability/unknown", cancellationToken);

    public Task<OrchestrationResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.DeleteAsync<OrchestrationResponse>($"{BasePath}/{id}", cancellationToken);

    // Queries
    public Task<OrchestrationResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<OrchestrationResponse>($"{BasePath}/{id}", cancellationToken);

    public Task<OrchestrationResponse> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<OrchestrationResponse>($"{BasePath}/by-name?Name={Encode(name)}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationResponse>>(BasePath, cancellationToken);

    public Task<IReadOnlyList<OrchestrationResponse>> GetByRegistrationStatusAsync(string status, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationResponse>>($"{BasePath}/by-registration-status/{Encode(status)}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationResponse>> GetByAvailabilityStatusAsync(string status, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationResponse>>($"{BasePath}/by-availability-status/{Encode(status)}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationResponse>> GetDeletedAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationResponse>>($"{BasePath}/deleted", cancellationToken);

    private static string Encode(string value) => Uri.EscapeDataString(value);
}
