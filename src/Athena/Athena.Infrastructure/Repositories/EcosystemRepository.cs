using Athena.Domain.Company.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Registry.Entities;
using Athena.Domain.Ecosystem.Registry.Enums;
using Athena.Domain.Ecosystem.Registry.Repositories;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class EcosystemRepository(AthenaDbContext context) : IEcosystemRepository
{
    private IQueryable<Ecosystem> ActiveEcosystems => context.Ecosystems.Where(ecosystem => !ecosystem.IsDeleted);

    public async Task<Ecosystem?> GetByIdAsync(EcosystemId id, CancellationToken cancellationToken = default)
    {
        return await ActiveEcosystems.FirstOrDefaultAsync(ecosystem => ecosystem.Id == id, cancellationToken);
    }

    public async Task<Ecosystem?> GetByNameAsync(EcosystemName name, CancellationToken cancellationToken = default)
    {
        return await ActiveEcosystems.FirstOrDefaultAsync(ecosystem => ecosystem.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<Ecosystem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await ActiveEcosystems.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Ecosystem>> GetByCompanyIdAsync(CompanyId companyId, CancellationToken cancellationToken = default)
    {
        return await ActiveEcosystems.Where(ecosystem => ecosystem.CompanyId == companyId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Ecosystem>> GetByRegistrationStatusAsync(EcosystemRegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await ActiveEcosystems.Where(ecosystem => ecosystem.RegistrationStatus == status).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Ecosystem>> GetByAvailabilityStatusAsync(EcosystemAvailabilityStatus status, CancellationToken cancellationToken = default)
    {
        return await ActiveEcosystems.Where(ecosystem => ecosystem.AvailabilityStatus == status).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Ecosystem>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default)
    {
        return await context.Ecosystems.Where(ecosystem => ecosystem.IsDeleted == isDeleted).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(EcosystemId id, CancellationToken cancellationToken = default)
    {
        return await context.Ecosystems.AnyAsync(ecosystem => ecosystem.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(EcosystemName name, CancellationToken cancellationToken = default)
    {
        return await context.Ecosystems.AnyAsync(ecosystem => ecosystem.Name == name, cancellationToken);
    }

    public async Task AddAsync(Ecosystem ecosystem, CancellationToken cancellationToken = default)
    {
        await context.Ecosystems.AddAsync(ecosystem, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Ecosystem ecosystem, CancellationToken cancellationToken = default)
    {
        context.Ecosystems.Update(ecosystem);
        await context.SaveChangesAsync(cancellationToken);
    }
}