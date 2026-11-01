using Athena.Domain.Company.ValueObjects;
using Athena.Domain.Ecosystem.Registry.Entities;
using Athena.Domain.Ecosystem.Registry.Enums;
using Athena.Domain.Ecosystem.Registry.Repositories;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Athena.Domain.Ecosystem.Management.Entities;
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

    public async Task AddAsync(Ecosystem ecosystem, EcosystemLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ecosystem);
        ArgumentNullException.ThrowIfNull(lifecycle);
        if (lifecycle.Id != ecosystem.Id)
            throw new ArgumentException("The lifecycle must belong to the Ecosystem being created.", nameof(lifecycle));

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var companyDatabaseId = await context.Companies
            .Where(company => company.Id == ecosystem.CompanyId && !company.IsDeleted)
            .Select(company => (int?)EF.Property<int>(company, "id"))
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Company '{ecosystem.CompanyId.Value}' does not exist.");
        context.Entry(ecosystem).Property<int>("company_database_id").CurrentValue = companyDatabaseId;
        await context.Ecosystems.AddAsync(ecosystem, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var ecosystemDatabaseId = await context.Ecosystems
            .Where(entity => entity.Id == ecosystem.Id)
            .Select(entity => EF.Property<int>(entity, "id"))
            .SingleAsync(cancellationToken);
        context.Entry(lifecycle).Property<int>("ecosystem_database_id").CurrentValue = ecosystemDatabaseId;
        await context.EcosystemLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateAsync(Ecosystem ecosystem, CancellationToken cancellationToken = default)
    {
        var ecosystemEntry = context.Entry(ecosystem);
        if (ecosystemEntry.State == EntityState.Detached) context.Ecosystems.Update(ecosystem);
        if (ecosystem.IsDeleted || ecosystem.RegistrationStatus != EcosystemRegistrationStatus.Registered)
        {
            var ecosystemDatabaseId = await context.Ecosystems
                .Where(entity => entity.Id == ecosystem.Id)
                .Select(entity => (int?)EF.Property<int>(entity, "id"))
                .SingleOrDefaultAsync(cancellationToken);
            if (ecosystemDatabaseId is not null)
            {
                var lifecycle = await context.EcosystemLifecycles.SingleOrDefaultAsync(
                    entity => EF.Property<int>(entity, "ecosystem_database_id") == ecosystemDatabaseId,
                    cancellationToken);
                if (lifecycle is not null && !lifecycle.IsDeleted)
                {
                    if (ecosystem.IsDeleted) lifecycle.Delete();
                    else lifecycle.SetOperationalStatus(Athena.Domain.Ecosystem.Management.Enums.EcosystemOperationalStatus.Unknown);
                }
            }
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}