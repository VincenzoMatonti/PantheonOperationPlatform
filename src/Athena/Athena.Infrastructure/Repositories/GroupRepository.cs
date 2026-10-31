using Athena.Domain.Company.Registry.Entities;
using Athena.Domain.Company.Registry.Enums;
using Athena.Domain.Company.Registry.Repositories;
using Athena.Domain.Company.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public class GroupRepository(AthenaDbContext dbContext) : IGroupRepository
{
    public async Task<Group?> GetByIdAsync(GroupId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await dbContext.Groups.FirstOrDefaultAsync(group => group.Id == id && !group.IsDeleted, cancellationToken);
    }

    public async Task<Group?> GetByNameAsync(GroupName name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        return await dbContext.Groups.FirstOrDefaultAsync(group => group.Name == name && !group.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Group>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Groups.Where(group => !group.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Group>> GetByRegistrationStatusAsync(GroupRegistrationStatus registrationStatus, CancellationToken cancellationToken = default)
    {
        return await dbContext.Groups.Where(group => group.RegistrationStatus == registrationStatus && !group.IsDeleted).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Group>> GetByDeletionStatusAsync(bool isDeleted, CancellationToken cancellationToken = default)
    {
        return await dbContext.Groups.Where(group => group.IsDeleted == isDeleted).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(GroupId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        return await dbContext.Groups.AnyAsync(group => group.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(GroupName name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        return await dbContext.Groups.AnyAsync(group => group.Name == name, cancellationToken);
    }

    public async Task AddAsync(Group group, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(group);
        await dbContext.Groups.AddAsync(group, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Group group, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(group);
        if (dbContext.Entry(group).State == EntityState.Detached) dbContext.Groups.Update(group);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
