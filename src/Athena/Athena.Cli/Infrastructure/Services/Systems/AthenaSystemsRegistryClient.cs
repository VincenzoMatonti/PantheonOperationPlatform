using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;
using Athena.Cli.Infrastructure.Services.Common;

namespace Athena.Cli.Infrastructure.Services.Systems;

public class AthenaSystemsRegistryClient(AthenaApiClient apiClient) : ISystemsRegistry
{
    private const string BasePath = "api/systems";

    public Task<SystemResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<SystemResponse>($"{BasePath}/{id}", cancellationToken);

    public Task<SystemResponse> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<SystemResponse>($"{BasePath}/by-name?Name={Encode(name)}", cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemResponse>>(BasePath, cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetByRegistrationStatusAsync(string status, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemResponse>>($"{BasePath}/by-registration-status/{Encode(status)}", cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetByAvailabilityStatusAsync(string status, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemResponse>>($"{BasePath}/by-availability-status/{Encode(status)}", cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetByCapabilityAsync(string capabilityName, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemResponse>>($"{BasePath}/by-capability?CapabilityName={Encode(capabilityName)}", cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemResponse>>($"{BasePath}/by-deletion-status?IsDeleted={isDeleted}", cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetDeletedAsync(CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemResponse>>($"{BasePath}/deleted", cancellationToken);

    public Task<IReadOnlyList<SystemEndpointResponse>> GetEndpointsAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemEndpointResponse>>($"{BasePath}/{id}/endpoints", cancellationToken);

    public Task<IReadOnlyList<SystemCapabilityResponse>> GetCapabilitiesAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.GetAsync<IReadOnlyList<SystemCapabilityResponse>>($"{BasePath}/{id}/capabilities", cancellationToken);

    public Task<SystemResponse> CreateAsync(RegisterSystemRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<RegisterSystemRequest, SystemResponse>(BasePath, request, cancellationToken);

    public Task<SystemResponse> RegisterAsync(RegisterSystemRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<RegisterSystemRequest, SystemResponse>($"{BasePath}/register", request, cancellationToken);

    public Task<SystemResponse> UnregisterAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<SystemResponse>($"{BasePath}/{id}/unregister", cancellationToken);

    public Task<SystemResponse> RenameAsync(Guid id, RenameSystemRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PatchAsync<RenameSystemRequest, SystemResponse>($"{BasePath}/{id}/name", request, cancellationToken);

    public Task<SystemResponse> EnableAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<SystemResponse>($"{BasePath}/{id}/enable", cancellationToken);

    public Task<SystemResponse> DisableAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<SystemResponse>($"{BasePath}/{id}/disable", cancellationToken);

    public Task<SystemResponse> MarkAvailableAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<SystemResponse>($"{BasePath}/{id}/availability/available", cancellationToken);

    public Task<SystemResponse> MarkUnavailableAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<SystemResponse>($"{BasePath}/{id}/availability/unavailable", cancellationToken);

    public Task<SystemResponse> MarkAvailabilityUnknownAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<SystemResponse>($"{BasePath}/{id}/availability/unknown", cancellationToken);

    public Task<SystemResponse> AddEndpointAsync(Guid id, AddSystemEndpointRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<AddSystemEndpointRequest, SystemResponse>($"{BasePath}/{id}/endpoints", request, cancellationToken);

    public Task<SystemResponse> RemoveEndpointAsync(Guid id, RemoveSystemEndpointRequest request, CancellationToken cancellationToken = default) =>
        apiClient.DeleteAsync<SystemResponse>($"{BasePath}/{id}/endpoints", cancellationToken);

    public Task<SystemResponse> AddCapabilityAsync(Guid id, AddSystemCapabilityRequest request, CancellationToken cancellationToken = default) =>
        apiClient.PostAsync<AddSystemCapabilityRequest, SystemResponse>($"{BasePath}/{id}/capabilities", request, cancellationToken);

    public Task<SystemResponse> RemoveCapabilityAsync(Guid id, RemoveSystemCapabilityRequest request, CancellationToken cancellationToken = default) =>
        apiClient.DeleteAsync<SystemResponse>($"{BasePath}/{id}/capabilities", cancellationToken);

    public Task<SystemResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default) =>
        apiClient.DeleteAsync<SystemResponse>($"{BasePath}/{id}", cancellationToken);

    private static string Encode(string value) => Uri.EscapeDataString(value);
}