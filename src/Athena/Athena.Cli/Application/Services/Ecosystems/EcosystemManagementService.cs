using Athena.Cli.Application.Contracts.Ecosystems;
using Athena.Cli.Application.Interfaces.Ecosystems;

namespace Athena.Cli.Application.Services.Ecosystems;

public sealed class EcosystemManagementService(IEcosystemManagement management)
{
    private readonly IEcosystemManagement _management = management;
    public Task<IReadOnlyList<EcosystemLifecycleResponse>> GetAllAsync(CancellationToken token = default) => _management.GetAllAsync(token);
    public Task<EcosystemLifecycleResponse?> GetAsync(Guid id, CancellationToken token = default) => _management.GetAsync(id, token);
    public Task<EcosystemLifecycleResponse> CreateAsync(Guid id, CancellationToken token = default) => _management.CreateAsync(id, token);
    public Task<EcosystemLifecycleResponse> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default) => _management.SetDesiredStateAsync(id, state, token);
    public Task<EcosystemOperationResponse> ExecuteAsync(Guid id, string operation, CancellationToken token = default) => _management.ExecuteAsync(id, operation, token);
    public Task<IReadOnlyList<EcosystemOperationResponse>> GetOperationsAsync(Guid id, CancellationToken token = default) => _management.GetOperationsAsync(id, token);
}
