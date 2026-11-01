using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Systems;

public sealed class AthenaSystemInstancesClient(AthenaApiClient apiClient) : ISystemInstances
{
    private const string BasePath = "api/system-instances/management";
    private readonly AthenaApiClient _apiClient = apiClient;

    public Task<IReadOnlyList<SystemInstanceProcessResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<IReadOnlyList<SystemInstanceProcessResponse>>(BasePath, cancellationToken);

    public async Task<SystemInstanceProcessResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _apiClient.GetAsync<SystemInstanceProcessResponse>($"{BasePath}/{id}", cancellationToken);
        }
        catch (AthenaApiException exception) when (exception.StatusCode == 404)
        {
            return null;
        }
    }

    public Task<SystemInstanceStatusResponse> CreateAsync(CreateSystemInstanceRequest request, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<CreateSystemInstanceRequest, SystemInstanceStatusResponse>(BasePath, request, cancellationToken);

    public Task<SystemInstanceStatusResponse> RegisterAsync(RegisterSystemInstanceRequest request, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<RegisterSystemInstanceRequest, SystemInstanceStatusResponse>($"{BasePath}/register", request, cancellationToken);

    public Task<SystemInstanceStatusResponse> StartAsync(Guid id, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<SystemInstanceStatusResponse>($"{BasePath}/{id}/start", cancellationToken);

    public Task<SystemInstanceStatusResponse> StopAsync(Guid id, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<SystemInstanceStatusResponse>($"{BasePath}/{id}/stop", cancellationToken);

    public Task<SystemInstanceStatusResponse> RestartAsync(Guid id, CancellationToken cancellationToken = default) =>
        _apiClient.PostAsync<SystemInstanceStatusResponse>($"{BasePath}/{id}/restart", cancellationToken);

    public Task<SystemInstanceStatusResponse> RemoveAsync(Guid id, CancellationToken cancellationToken = default) =>
        _apiClient.DeleteAsync<SystemInstanceStatusResponse>($"{BasePath}/{id}", cancellationToken);

    public Task<SystemInstanceStatusResponse> GetStatusAsync(Guid id, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<SystemInstanceStatusResponse>($"{BasePath}/{id}/status", cancellationToken);

    public Task<SystemInstanceHealthResponse> GetHealthAsync(Guid id, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<SystemInstanceHealthResponse>($"{BasePath}/{id}/health", cancellationToken);

    public Task<IReadOnlyList<SystemInstanceLogResponse>> GetLogsAsync(Guid id, int tail = 100, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<IReadOnlyList<SystemInstanceLogResponse>>($"{BasePath}/{id}/logs?tail={tail}", cancellationToken);

    public Task<IReadOnlyList<SystemInstanceOperationResponse>> GetOperationsAsync(Guid id, CancellationToken cancellationToken = default) =>
        _apiClient.GetAsync<IReadOnlyList<SystemInstanceOperationResponse>>($"{BasePath}/{id}/operations", cancellationToken);
}
