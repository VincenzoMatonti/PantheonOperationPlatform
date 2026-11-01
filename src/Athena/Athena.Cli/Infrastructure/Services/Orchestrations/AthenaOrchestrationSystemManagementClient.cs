using Athena.Cli.Application.Contracts.Orchestrations;
using Athena.Cli.Application.Interfaces.Orchestrations;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Orchestrations;

public class AthenaOrchestrationSystemInstanceManagementClient(AthenaApiClient apiClient) : IOrchestrationSystemInstanceManagement
{
    private const string BasePath = "api/orchestrations/management/system-instances";

    public Task<OrchestrationSystemInstanceResponse> CreateAsync(CreateOrchestrationSystemInstanceRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<CreateOrchestrationSystemInstanceRequest, OrchestrationSystemInstanceResponse>(BasePath, request, cancellationToken);

    public Task<OrchestrationSystemInstanceResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.DeleteAsync<OrchestrationSystemInstanceResponse>($"{BasePath}/{id}", cancellationToken);

    public Task<OrchestrationSystemInstanceResponse> RestoreAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<OrchestrationSystemInstanceResponse>($"{BasePath}/{id}/restore", cancellationToken);

    public Task<OrchestrationSystemInstanceResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<OrchestrationSystemInstanceResponse>($"{BasePath}/{id}", cancellationToken);

    public Task<IReadOnlyList<OrchestrationSystemInstanceResponse>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<OrchestrationSystemInstanceResponse>>(
            $"{BasePath}?includeDeleted={includeDeleted.ToString().ToLowerInvariant()}", cancellationToken);
}
