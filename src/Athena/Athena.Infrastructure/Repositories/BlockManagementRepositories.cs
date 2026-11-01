using Athena.Domain.Block.Management.Entities;
using Athena.Domain.Block.Management.Repositories;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.Infrastructure.Repositories;

public sealed class BlockLifecycleRepository(AthenaDbContext context) : IBlockLifecycleRepository
{
    public async Task<BlockLifecycle?> GetByIdAsync(BlockId id, CancellationToken cancellationToken = default)
    {
        var dbId = await FindDatabaseIdAsync(id, cancellationToken);
        if (dbId is null) return null;
        return await context.BlockLifecycles.SingleOrDefaultAsync(x => EF.Property<int>(x, "block_database_id") == dbId, cancellationToken);
    }
    public async Task<IReadOnlyList<BlockLifecycle>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.BlockLifecycles.Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
    public async Task AddAsync(BlockLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        var dbId = await FindDatabaseIdAsync(lifecycle.Id, cancellationToken) ?? throw new InvalidOperationException($"Block '{lifecycle.Id.Value}' does not exist.");
        context.Entry(lifecycle).Property<int>("block_database_id").CurrentValue = dbId;
        await context.BlockLifecycles.AddAsync(lifecycle, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(BlockLifecycle lifecycle, CancellationToken cancellationToken = default)
    {
        var dbId = await FindDatabaseIdAsync(lifecycle.Id, cancellationToken) ?? throw new InvalidOperationException($"Block '{lifecycle.Id.Value}' does not exist.");
        var entry = context.Entry(lifecycle);
        if (entry.State == EntityState.Detached) { context.BlockLifecycles.Update(lifecycle); entry = context.Entry(lifecycle); }
        entry.Property<int>("block_database_id").CurrentValue = dbId;
        await context.SaveChangesAsync(cancellationToken);
    }
    private Task<int?> FindDatabaseIdAsync(BlockId id, CancellationToken token) =>
        context.Blocks.Where(x => x.Id == id).Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(token);
}

public sealed class BlockOperationRepository(AthenaDbContext context) : IBlockOperationRepository
{
    public async Task AddAsync(BlockOperation operation, CancellationToken cancellationToken = default)
    {
        var dbId = await context.Blocks.Where(x => x.Id == operation.BlockId)
            .Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Block '{operation.BlockId.Value}' does not exist.");
        context.Entry(operation).Property<int>("block_database_id").CurrentValue = dbId;
        await context.BlockOperations.AddAsync(operation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task SaveIntentAsync(
        BlockOperation operation,
        BlockLifecycle lifecycle,
        CancellationToken cancellationToken = default)
    {
        if (operation.BlockId != lifecycle.Id)
            throw new ArgumentException("The lifecycle must belong to the operation's Block.", nameof(lifecycle));

        var dbId = await context.Blocks.Where(x => x.Id == operation.BlockId)
            .Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Block '{operation.BlockId.Value}' does not exist.");
        context.Entry(operation).Property<int>("block_database_id").CurrentValue = dbId;
        var lifecycleEntry = context.Entry(lifecycle);
        if (lifecycleEntry.State == EntityState.Detached)
        {
            context.BlockLifecycles.Update(lifecycle);
            lifecycleEntry = context.Entry(lifecycle);
        }
        lifecycleEntry.Property<int>("block_database_id").CurrentValue = dbId;
        await context.BlockOperations.AddAsync(operation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateAsync(BlockOperation operation, CancellationToken cancellationToken = default)
    {
        var dbId = await context.Blocks.Where(x => x.Id == operation.BlockId)
            .Select(x => (int?)EF.Property<int>(x, "id")).SingleOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"Block '{operation.BlockId.Value}' does not exist.");
        var entry = context.Entry(operation);
        if (entry.State == EntityState.Detached) { context.BlockOperations.Update(operation); entry = context.Entry(operation); }
        entry.Property<int>("block_database_id").CurrentValue = dbId;
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<BlockOperation>> GetByBlockIdAsync(BlockId id, CancellationToken cancellationToken = default) =>
        await context.BlockOperations.AsNoTracking().Where(x => x.BlockId == id)
            .OrderByDescending(x => x.CreatedAtUtc).ToListAsync(cancellationToken);
}
