using Athena.Domain.SystemRegistry.Enums;
using Athena.Domain.SystemRegistry.ValueObjects;
using RegisteredSystem = Athena.Domain.SystemRegistry.Entities.System;

namespace Athena.Domain.SystemRegistry.Repositories;

public interface ISystemRepository
{
    // Retrieval
    Task<RegisteredSystem?> GetByIdAsync(SystemId id, CancellationToken cancellationToken = default);
    Task<RegisteredSystem?> GetByNameAsync(SystemName name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RegisteredSystem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RegisteredSystem>> GetByRegistrationStatusAsync(SystemRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RegisteredSystem>> GetByAvailabilityStatusAsync(SystemAvailabilityStatus status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RegisteredSystem>> GetByCapabilityNameAsync(string capabilityName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemEndpoint>> GetAllEndpointsAsync(SystemId systemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemCapability>> GetAllCapabilitiesAsync(SystemId systemId, CancellationToken cancellationToken = default);

    // Soft deletion
    Task<IReadOnlyList<RegisteredSystem>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default);

    // Existence checks
    Task<bool> ExistsByIdAsync(SystemId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(SystemName name, CancellationToken cancellationToken = default);

    // Persistence
    Task AddAsync(RegisteredSystem system, CancellationToken cancellationToken = default);
    Task UpdateAsync(RegisteredSystem system, CancellationToken cancellationToken = default);
}