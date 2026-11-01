using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;

namespace Athena.Cli.Application.Services.Systems;

public sealed class SystemInstancesService(ISystemInstances systemInstances)
{
    private readonly ISystemInstances _systemInstances = systemInstances;

    public Task<IReadOnlyList<SystemInstanceProcessResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _systemInstances.GetAllAsync(cancellationToken);

    public Task<SystemInstanceProcessResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _systemInstances.GetByIdAsync(id, cancellationToken);

    public Task<SystemInstanceStatusResponse> CreateAsync(CreateSystemInstanceRequest request, CancellationToken cancellationToken = default) =>
        _systemInstances.CreateAsync(request, cancellationToken);

    public Task<SystemInstanceStatusResponse> RegisterAsync(RegisterSystemInstanceRequest request, CancellationToken cancellationToken = default) =>
        _systemInstances.RegisterAsync(request, cancellationToken);

    public Task<SystemInstanceOperationResponse> StartAsync(Guid id, CancellationToken cancellationToken = default) =>
        _systemInstances.StartAsync(id, cancellationToken);

    public Task<SystemInstanceOperationResponse> StopAsync(Guid id, CancellationToken cancellationToken = default) =>
        _systemInstances.StopAsync(id, cancellationToken);

    public Task<SystemInstanceOperationResponse> RestartAsync(Guid id, CancellationToken cancellationToken = default) =>
        _systemInstances.RestartAsync(id, cancellationToken);

    public Task<SystemInstanceStatusResponse> RemoveAsync(Guid id, CancellationToken cancellationToken = default) =>
        _systemInstances.RemoveAsync(id, cancellationToken);

    public Task<SystemInstanceStatusResponse> GetStatusAsync(Guid id, CancellationToken cancellationToken = default) =>
        _systemInstances.GetStatusAsync(id, cancellationToken);

    public Task<SystemInstanceHealthResponse> GetHealthAsync(Guid id, CancellationToken cancellationToken = default) =>
        _systemInstances.GetHealthAsync(id, cancellationToken);

    public Task<IReadOnlyList<SystemInstanceLogResponse>> GetLogsAsync(Guid id, int tail = 100, CancellationToken cancellationToken = default) =>
        _systemInstances.GetLogsAsync(id, tail, cancellationToken);

    public Task<IReadOnlyList<SystemInstanceOperationResponse>> GetOperationsAsync(Guid id, CancellationToken cancellationToken = default) =>
        _systemInstances.GetOperationsAsync(id, cancellationToken);

    public Task<SystemInstanceOperationResponse?> GetOperationAsync(Guid id, Guid operationId, CancellationToken cancellationToken = default) =>
        _systemInstances.GetOperationAsync(id, operationId, cancellationToken);
}
