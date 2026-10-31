using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Interfaces.Orchestrations;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Orchestrations;

public class AthenaOrchestrationSystemManagementClient(AthenaApiClient apiClient) : IOrchestrationSystemManagement
{
    private const string BasePath = "api/orchestrations/management/systems";

    public Task<OrchestrationSystemResponse> CreateAsync(CreateOrchestrationSystemRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<CreateOrchestrationSystemRequest, OrchestrationSystemResponse>(BasePath, request, cancellationToken);

    public Task<OrchestrationSystemResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.DeleteAsync<OrchestrationSystemResponse>($"{BasePath}/{id}", cancellationToken);

    public Task<OrchestrationSystemResponse> RestoreAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationSystemResponse>($"{BasePath}/{id}/restore", cancellationToken);

    public Task<OrchestrationSystemResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<OrchestrationSystemResponse>($"{BasePath}/{id}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationSystemResponse>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationSystemResponse>>(
            $"{BasePath}?includeDeleted={includeDeleted.ToString().ToLowerInvariant()}", cancellationToken);
}
