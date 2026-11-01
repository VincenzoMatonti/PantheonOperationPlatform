using Athena.Cli.Application.Contracts.Systems;

namespace Athena.Cli.Application.Interfaces.Systems;

public interface ISystemInstances
{
    Task<IReadOnlyList<SystemInstanceProcessResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SystemInstanceProcessResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemInstanceStatusResponse> CreateAsync(CreateSystemInstanceRequest request, CancellationToken cancellationToken = default);
    Task<SystemInstanceStatusResponse> RegisterAsync(RegisterSystemInstanceRequest request, CancellationToken cancellationToken = default);
    Task<SystemInstanceOperationResponse> StartAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemInstanceOperationResponse> StopAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemInstanceOperationResponse> RestartAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemInstanceStatusResponse> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemInstanceStatusResponse> GetStatusAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemInstanceHealthResponse> GetHealthAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstanceLogResponse>> GetLogsAsync(Guid id, int tail = 100, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemInstanceOperationResponse>> GetOperationsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemInstanceOperationResponse?> GetOperationAsync(Guid id, Guid operationId, CancellationToken cancellationToken = default);
}
