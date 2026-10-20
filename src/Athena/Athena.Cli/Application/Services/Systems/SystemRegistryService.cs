using Athena.Cli.Application.Contracts.Systems;
using Athena.Cli.Application.Interfaces.Systems;

namespace Athena.Cli.Application.Services.Systems;

public sealed class SystemRegistryService(ISystemsRegistry systemsRegistry)
{
    public Task<SystemResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        systemsRegistry.GetByIdAsync(id, cancellationToken);

    public Task<SystemResponse> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        systemsRegistry.GetByNameAsync(name, cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        systemsRegistry.GetAllAsync(cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetByRegistrationStatusAsync(string status, CancellationToken cancellationToken = default) =>
        systemsRegistry.GetByRegistrationStatusAsync(status, cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetByAvailabilityStatusAsync(string status, CancellationToken cancellationToken = default) =>
        systemsRegistry.GetByAvailabilityStatusAsync(status, cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetByCapabilityAsync(string capabilityName, CancellationToken cancellationToken = default) =>
        systemsRegistry.GetByCapabilityAsync(capabilityName, cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default) =>
        systemsRegistry.GetByDeletionStatusAsync(isDeleted, cancellationToken);

    public Task<IReadOnlyList<SystemResponse>> GetDeletedAsync(CancellationToken cancellationToken = default) =>
        systemsRegistry.GetDeletedAsync(cancellationToken);

    public Task<IReadOnlyList<SystemEndpointResponse>> GetEndpointsAsync(Guid id, CancellationToken cancellationToken = default) =>
        systemsRegistry.GetEndpointsAsync(id, cancellationToken);

    public Task<IReadOnlyList<SystemCapabilityResponse>> GetCapabilitiesAsync(Guid id, CancellationToken cancellationToken = default) =>
        systemsRegistry.GetCapabilitiesAsync(id, cancellationToken);

    public Task<SystemResponse> CreateAsync(RegisterSystemRequest request, CancellationToken cancellationToken = default) =>
        systemsRegistry.CreateAsync(request, cancellationToken);

    public Task<SystemResponse> RegisterAsync(RegisterSystemRequest request, CancellationToken cancellationToken = default) =>
        systemsRegistry.RegisterAsync(request, cancellationToken);

    public Task<SystemResponse> UnregisterAsync(Guid id, CancellationToken cancellationToken = default) =>
        systemsRegistry.UnregisterAsync(id, cancellationToken);

    public Task<SystemResponse> RenameAsync(Guid id, RenameSystemRequest request, CancellationToken cancellationToken = default) =>
        systemsRegistry.RenameAsync(id, request, cancellationToken);

    public Task<SystemResponse> EnableAsync(Guid id, CancellationToken cancellationToken = default) =>
        systemsRegistry.EnableAsync(id, cancellationToken);

    public Task<SystemResponse> DisableAsync(Guid id, CancellationToken cancellationToken = default) =>
        systemsRegistry.DisableAsync(id, cancellationToken);

    public Task<SystemResponse> MarkAvailableAsync(Guid id, CancellationToken cancellationToken = default) =>
        systemsRegistry.MarkAvailableAsync(id, cancellationToken);

    public Task<SystemResponse> MarkUnavailableAsync(Guid id, CancellationToken cancellationToken = default) =>
        systemsRegistry.MarkUnavailableAsync(id, cancellationToken);

    public Task<SystemResponse> MarkAvailabilityUnknownAsync(Guid id, CancellationToken cancellationToken = default) =>
        systemsRegistry.MarkAvailabilityUnknownAsync(id, cancellationToken);

    public Task<SystemResponse> AddEndpointAsync(Guid id, AddSystemEndpointRequest request, CancellationToken cancellationToken = default) =>
        systemsRegistry.AddEndpointAsync(id, request, cancellationToken);

    public Task<SystemResponse> RemoveEndpointAsync(Guid id, RemoveSystemEndpointRequest request, CancellationToken cancellationToken = default) =>
        systemsRegistry.RemoveEndpointAsync(id, request, cancellationToken);

    public Task<SystemResponse> AddCapabilityAsync(Guid id, AddSystemCapabilityRequest request, CancellationToken cancellationToken = default) =>
        systemsRegistry.AddCapabilityAsync(id, request, cancellationToken);

    public Task<SystemResponse> RemoveCapabilityAsync(Guid id, RemoveSystemCapabilityRequest request, CancellationToken cancellationToken = default) =>
         systemsRegistry.RemoveCapabilityAsync(id, request, cancellationToken);
    public Task<SystemResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default) => systemsRegistry.DeleteAsync(id, cancellationToken);
}
