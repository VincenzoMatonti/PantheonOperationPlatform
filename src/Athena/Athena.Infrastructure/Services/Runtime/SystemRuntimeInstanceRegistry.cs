using Athena.Application.Common.Dtos;
using Athena.Application.Common.Interfaces;
using Athena.Domain.SystemRegistry.ValueObjects;
using Athena.Infrastructure.Persistence;
using PersistenceRuntimeInstance = Athena.Infrastructure.Persistence.Runtime.SystemRuntimeInstance;
using Microsoft.EntityFrameworkCore;
namespace Athena.Infrastructure.Services.Runtime;

public sealed class SystemRuntimeInstanceRegistry(AthenaDbContext dbContext) : ISystemRuntimeInstanceRegistry
{
    public async Task<SystemRuntimeInstanceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var instance = await dbContext.SystemRuntimeInstances.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        return instance is null ? null : ToDto(instance);
    }

    public async Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetBySystemIdAsync(Guid systemId, CancellationToken cancellationToken = default)
    {
        EnsureSystemId(systemId);
        var id = SystemId.Create(systemId);
        var instances = await dbContext.SystemRuntimeInstances
            .AsNoTracking()
            .Where(x => x.SystemId == id)
            .OrderBy(x => x.Environment)
            .ThenBy(x => x.RuntimeProvider)
            .ToListAsync(cancellationToken);
        return [.. instances.Select(ToDto)];
    }

    public async Task<IReadOnlyList<SystemRuntimeInstanceDto>> GetBySystemIdAndEnvironmentAsync(
        Guid systemId, string environment, CancellationToken cancellationToken = default)
    {
        EnsureSystemId(systemId);
        EnsureText(environment, nameof(environment), 100);
        var id = SystemId.Create(systemId);
        var normalizedEnvironment = environment.Trim();
        var instances = await dbContext.SystemRuntimeInstances
            .AsNoTracking().Where(x => x.SystemId == id && x.Environment == normalizedEnvironment).OrderBy(x => x.RuntimeProvider).ToListAsync(cancellationToken);
        return instances.Select(ToDto).ToArray();
    }

    public async Task<SystemRuntimeInstanceDto> RegisterAsync(
        Guid systemId, string runtimeProvider, string runtimeResourceId, string runtimeResourceName, string environment, CancellationToken cancellationToken = default)
    {
        EnsureSystemId(systemId);
        EnsureText(runtimeProvider, nameof(runtimeProvider), 50);
        EnsureText(runtimeResourceId, nameof(runtimeResourceId), 255);
        EnsureText(runtimeResourceName, nameof(runtimeResourceName), 255);
        EnsureText(environment, nameof(environment), 100);
        runtimeProvider = runtimeProvider.Trim();
        runtimeResourceId = runtimeResourceId.Trim();
        runtimeResourceName = runtimeResourceName.Trim();
        environment = environment.Trim();
        var domainSystemId = SystemId.Create(systemId);
        var system = await dbContext.Systems.SingleOrDefaultAsync(x => x.Id == domainSystemId, cancellationToken) ??
        throw new InvalidOperationException($"Il sistema '{systemId}' non è registrato in Athena.");
        var duplicate = await dbContext.SystemRuntimeInstances
            .AnyAsync(x => x.RuntimeProvider == runtimeProvider && x.Environment == environment && x.RuntimeResourceId == runtimeResourceId, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException(
                $"La risorsa runtime '{runtimeResourceId}' è già registrata " + $"per il provider '{runtimeProvider}' nell'ambiente '{environment}'.");

        var now = DateTime.UtcNow;
        var instance = new PersistenceRuntimeInstance
        {
            Id = Guid.NewGuid(),
            SystemId = domainSystemId,
            RuntimeProvider = runtimeProvider,
            RuntimeResourceId = runtimeResourceId,
            RuntimeResourceName = runtimeResourceName,
            Environment = environment,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        // La FK include la chiave tecnica shadow "id" della tabella systems.
        var systemDatabaseId = dbContext.Entry(system).Property<int>("id").CurrentValue;
        dbContext.SystemRuntimeInstances.Add(instance);
        dbContext.Entry(instance).Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(instance);
    }

    public async Task<bool> UnregisterAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var instance = await dbContext.SystemRuntimeInstances.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (instance is null) return false;

        // Elimina soltanto la registrazione Athena, non la risorsa esterna.
        dbContext.SystemRuntimeInstances.Remove(instance);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static SystemRuntimeInstanceDto ToDto(PersistenceRuntimeInstance instance)
        => new(
            instance.Id,
            instance.SystemId.Value,
            instance.RuntimeProvider,
            instance.RuntimeResourceId,
            instance.RuntimeResourceName,
            instance.Environment,
            instance.CreatedAtUtc,
            instance.UpdatedAtUtc);

    private static void EnsureSystemId(Guid systemId)
    {
        if (systemId == Guid.Empty) throw new ArgumentException("Il system ID non può essere vuoto.", nameof(systemId));
    }

    private static void EnsureText(string value, string parameterName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Il valore è obbligatorio.", parameterName);
        if (value.Trim().Length > maxLength) throw new ArgumentException($"Il valore non può superare {maxLength} caratteri.", parameterName);
    }
}
