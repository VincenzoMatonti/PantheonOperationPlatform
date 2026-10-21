using Athena.Cli.Application.Contracts.Systems;

namespace Athena.Cli.Application.Interfaces.Systems;

public interface ISystemsRegistry
{
    Task<SystemResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemResponse> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemResponse>> GetByRegistrationStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemResponse>> GetByAvailabilityStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemResponse>> GetByCapabilityAsync(string capabilityName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemResponse>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemResponse>> GetDeletedAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SystemEndpointResponse>> GetEndpointsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemCapabilityResponse>> GetCapabilitiesAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SystemResponse> CreateAsync(RegisterSystemRequest request, CancellationToken cancellationToken = default);
    Task<SystemResponse> RegisterAsync(RegisterSystemRequest request, CancellationToken cancellationToken = default);
    Task<SystemResponse> UnregisterAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemResponse> RenameAsync(Guid id, RenameSystemRequest request, CancellationToken cancellationToken = default);
    Task<SystemResponse> EnableAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemResponse> DisableAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SystemResponse> MarkAvailableAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemResponse> MarkUnavailableAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SystemResponse> MarkAvailabilityUnknownAsync(Guid id, CancellationToken cancellationToken = default);

    Task<SystemResponse> AddEndpointAsync(Guid id, AddSystemEndpointRequest request, CancellationToken cancellationToken = default);
    Task<SystemResponse> RemoveEndpointAsync(Guid id, RemoveSystemEndpointRequest request, CancellationToken cancellationToken = default);
    Task<SystemResponse> AddCapabilityAsync(Guid id, AddSystemCapabilityRequest request, CancellationToken cancellationToken = default);
    Task<SystemResponse> RemoveCapabilityAsync(Guid id, RemoveSystemCapabilityRequest request, CancellationToken cancellationToken = default);
    Task<SystemResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}