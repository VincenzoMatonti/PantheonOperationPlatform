using Athena.Domain.OrchestrationRegistry.Entities;
using Athena.Domain.OrchestrationRegistry.Enums;
using Athena.Domain.OrchestrationRegistry.Repositories;
using Athena.Domain.OrchestrationRegistry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class OrchestrationRepository(AthenaDbContext context) : IOrchestrationRepository
{
    public async Task<Orchestration?> GetByIdAsync(OrchestrationId id, CancellationToken cancellationToken = default)
    {
        return await context.Orchestrations.FirstOrDefaultAsync(orchestration => orchestration.Id == id && !orchestration.IsDeleted, cancellationToken);
    }

    public async Task<Orchestration?> GetByNameAsync(OrchestrationName name, CancellationToken cancellationToken = default)
    {
        return await context.Orchestrations.FirstOrDefaultAsync(orchestration => orchestration.Name == name && !orchestration.IsDeleted, cancellationToken);
    }

    public async Task<List<Orchestration>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Orchestrations.Where(orchestration => !orchestration.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<List<Orchestration>> GetByRegistrationStatusAsync(OrchestrationRegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await context.Orchestrations
            .Where(orchestration => !orchestration.IsDeleted && orchestration.RegistrationStatus == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Orchestration>> GetByAvailabilityStatusAsync(OrchestrationAvailabilityStatus status, CancellationToken cancellationToken = default)
    {
        return await context.Orchestrations
            .Where(orchestration => !orchestration.IsDeleted && orchestration.AvailabilityStatus == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Orchestration>> GetDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await context.Orchestrations.Where(orchestration => orchestration.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Orchestration orchestration, CancellationToken cancellationToken = default)
    {
        await context.Orchestrations.AddAsync(orchestration, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Orchestration orchestration, CancellationToken cancellationToken = default)
    {
        context.Orchestrations.Update(orchestration);
        await context.SaveChangesAsync(cancellationToken);
    }
}
