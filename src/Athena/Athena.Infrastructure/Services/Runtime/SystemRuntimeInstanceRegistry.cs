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

    public async Task<SystemRuntimeInstanceDto?> SynchronizeDiscoveredAsync(
        DiscoveredRuntimeInstanceDto discoveredInstance, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(discoveredInstance);
        EnsureSystemId(discoveredInstance.SystemId);
        EnsureText(discoveredInstance.RuntimeProvider, nameof(discoveredInstance.RuntimeProvider), 50);
        EnsureText(discoveredInstance.RuntimeResourceId, nameof(discoveredInstance.RuntimeResourceId), 255);
        EnsureText(discoveredInstance.RuntimeResourceName, nameof(discoveredInstance.RuntimeResourceName), 255);
        EnsureText(discoveredInstance.Environment, nameof(discoveredInstance.Environment), 100);
        var systemId = SystemId.Create(discoveredInstance.SystemId);
        var runtimeProvider = discoveredInstance.RuntimeProvider.Trim();
        var runtimeResourceId = discoveredInstance.RuntimeResourceId.Trim();
        var runtimeResourceName = discoveredInstance.RuntimeResourceName.Trim();
        var environment = discoveredInstance.Environment.Trim();
        var system = await dbContext.Systems.SingleOrDefaultAsync(x => x.Id == systemId, cancellationToken);
        if (system is null) return null;
        var systemDatabaseId = dbContext.Entry(system).Property<int>("id").CurrentValue;
        var instance = await dbContext.SystemRuntimeInstances.SingleOrDefaultAsync(
                x => x.RuntimeProvider == runtimeProvider && x.Environment == environment && x.RuntimeResourceId == runtimeResourceId, cancellationToken);

        // Docker may assign a new ID when Compose recreates a container.
        // The name is the fallback identity within the same provider/environment.
        instance ??= await dbContext.SystemRuntimeInstances.SingleOrDefaultAsync(
                x => x.RuntimeProvider == runtimeProvider && x.Environment == environment && x.RuntimeResourceName == runtimeResourceName, cancellationToken);
        var now = DateTime.UtcNow;
        if (instance is null)
        {
            instance = new PersistenceRuntimeInstance
            {
                Id = Guid.NewGuid(),
                SystemId = systemId,
                RuntimeProvider = runtimeProvider,
                RuntimeResourceId = runtimeResourceId,
                RuntimeResourceName = runtimeResourceName,
                Environment = environment,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            dbContext.SystemRuntimeInstances.Add(instance);
            dbContext.Entry(instance).Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        }
        else
        {
            instance.SystemId = systemId;
            instance.RuntimeProvider = runtimeProvider;
            instance.RuntimeResourceId = runtimeResourceId;
            instance.RuntimeResourceName = runtimeResourceName;
            instance.Environment = environment;
            instance.UpdatedAtUtc = now;
            dbContext.Entry(instance).Property<int>("system_database_id").CurrentValue = systemDatabaseId;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return ToDto(instance);
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
