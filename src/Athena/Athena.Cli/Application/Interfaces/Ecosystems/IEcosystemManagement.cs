using Athena.Cli.Application.Contracts.Ecosystems;

namespace Athena.Cli.Application.Interfaces.Ecosystems;

public interface IEcosystemManagement
{
    Task<IReadOnlyList<EcosystemLifecycleResponse>> GetAllAsync(CancellationToken token = default);
    Task<EcosystemLifecycleResponse?> GetAsync(Guid id, CancellationToken token = default);
    Task<EcosystemLifecycleResponse> CreateAsync(Guid id, CancellationToken token = default);
    Task<EcosystemLifecycleResponse> SetDesiredStateAsync(Guid id, string state, CancellationToken token = default);
    Task<EcosystemOperationResponse> ExecuteAsync(Guid id, string operation, CancellationToken token = default);
    Task<IReadOnlyList<EcosystemOperationResponse>> GetOperationsAsync(Guid id, CancellationToken token = default);
}
