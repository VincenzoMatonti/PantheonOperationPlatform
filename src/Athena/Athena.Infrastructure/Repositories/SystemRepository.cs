using Microsoft.EntityFrameworkCore;
using Athena.Infrastructure.Persistence;
using Athena.Domain.SystemRegistry.Enums;
using Athena.Domain.SystemRegistry.Repositories;
using Athena.Domain.SystemRegistry.ValueObjects;
using RegisteredSystem = Athena.Domain.SystemRegistry.Entities.System;

namespace Athena.Infrastructure.Repositories;

public sealed class SystemRepository(AthenaDbContext dbContext) : ISystemRepository
{
    private IQueryable<RegisteredSystem> SystemsWithDetails => dbContext.Systems
        .Include(system => system.Endpoints)
        .Include(system => system.Capabilities);

    // Retrieval
    public async Task<RegisteredSystem?> GetByIdAsync(SystemId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await SystemsWithDetails.SingleOrDefaultAsync(system => system.Id == id, cancellationToken);
    }

    public async Task<RegisteredSystem?> GetByNameAsync(SystemName name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        return await SystemsWithDetails.SingleOrDefaultAsync(system => system.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await SystemsWithDetails.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetByRegistrationStatusAsync(SystemRegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await SystemsWithDetails.Where(system => system.RegistrationStatus == status).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetByAvailabilityStatusAsync(SystemAvailabilityStatus status, CancellationToken cancellationToken = default)
    {
        return await SystemsWithDetails.Where(system => system.AvailabilityStatus == status).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RegisteredSystem>> GetByCapabilityNameAsync(string capabilityName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(capabilityName);
        var normalizedName = capabilityName.Trim();
        return await SystemsWithDetails.Where(system => system.Capabilities.Any(capability => capability.Name == normalizedName)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemEndpoint>> GetAllEndpointsAsync(SystemId systemId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        var system = await SystemsWithDetails.SingleOrDefaultAsync(item => item.Id == systemId, cancellationToken);
        return system?.Endpoints.ToList() ?? [];
    }

    public async Task<IReadOnlyList<SystemCapability>> GetAllCapabilitiesAsync(SystemId systemId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(systemId);
        var system = await SystemsWithDetails.SingleOrDefaultAsync(item => item.Id == systemId, cancellationToken);
        return system?.Capabilities.ToList() ?? [];
    }

    // Soft deletion
    public async Task<IReadOnlyList<RegisteredSystem>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default)
    {
        return await SystemsWithDetails.Where(system => system.IsDeleted == isDeleted).ToListAsync(cancellationToken);
    }

    // Existence checks
    public async Task<bool> ExistsByIdAsync(SystemId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await dbContext.Systems.AnyAsync(system => system.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(SystemName name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        return await dbContext.Systems.AnyAsync(system => system.Name == name, cancellationToken);
    }

    // Persistence
    public async Task AddAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(system);
        await dbContext.Systems.AddAsync(system, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RegisteredSystem system, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(system);
        var entry = dbContext.Entry(system);
        if (entry.State == EntityState.Detached) dbContext.Systems.Update(system);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}