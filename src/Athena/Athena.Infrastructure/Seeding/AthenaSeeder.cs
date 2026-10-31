using Microsoft.Extensions.Logging;
using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.Entities;
using Athena.Domain.Orchestration.Registry.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Repositories;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;

namespace Athena.Infrastructure.Seeding;

public sealed class AthenaSeeder(
    ISystemRepository systemRepository,
    ISystemLifecycleRepository systemLifecycleRepository,
    IOrchestrationRepository orchestrationRepository,
    IOrchestrationLifecycleRepository orchestrationLifecycleRepository,
    IOrchestrationSystemRepository orchestrationSystemRepository,
    ILogger<AthenaSeeder> logger)
{
    private readonly ISystemRepository _systemRepository = systemRepository;
    private readonly ISystemLifecycleRepository _systemLifecycleRepository = systemLifecycleRepository;
    private readonly IOrchestrationRepository _orchestrationRepository = orchestrationRepository;
    private readonly IOrchestrationLifecycleRepository _orchestrationLifecycleRepository = orchestrationLifecycleRepository;
    private readonly IOrchestrationSystemRepository _orchestrationSystemRepository = orchestrationSystemRepository;
    private readonly ILogger<AthenaSeeder> _logger = logger;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Athena database seeding.");
        var systems = await SeedSystemsAsync(cancellationToken);
        await SeedOrchestrationsAsync(systems, cancellationToken);
        _logger.LogInformation("Athena database seeding completed.");
    }

    private async Task<Dictionary<string, RegisteredSystem>> SeedSystemsAsync(CancellationToken cancellationToken)
    {
        var systems = new Dictionary<string, RegisteredSystem>(StringComparer.OrdinalIgnoreCase);
        foreach (var definition in AthenaSeedData.Systems)
        {
            var name = SystemName.Create(definition.Name);
            var system = await _systemRepository.GetByNameAsync(name, cancellationToken);
            if (system is null)
            {
                var id = SystemId.Create(definition.Id);
                system = RegisteredSystem.Create(id, name);
                await _systemRepository.AddAsync(system, cancellationToken);
                _logger.LogInformation("Registered initial system {SystemName}.", definition.Name);
            }

            var lifecycle = await _systemLifecycleRepository.GetBySystemIdAsync(system.Id, cancellationToken);
            if (lifecycle is null)
            {
                lifecycle = SystemLifecycle.Create(system.Id);
                await _systemLifecycleRepository.AddAsync(lifecycle, cancellationToken);
                _logger.LogInformation("Created lifecycle for system {SystemName}.", definition.Name);
            }

            systems.Add(definition.Name, system);
        }

        return systems;
    }

    private async Task SeedOrchestrationsAsync(IReadOnlyDictionary<string, RegisteredSystem> systems, CancellationToken cancellationToken)
    {
        foreach (var definition in AthenaSeedData.Orchestrations)
        {
            var id = OrchestrationId.Create(definition.Id);
            var name = OrchestrationName.Create(definition.Name);
            var orchestration = await _orchestrationRepository.GetByIdAsync(id, cancellationToken) ??
            await _orchestrationRepository.GetByNameAsync(name, cancellationToken);
            if (orchestration is null)
            {
                orchestration = Orchestration.Create(id, name);
                await _orchestrationRepository.AddAsync(orchestration, cancellationToken);
                _logger.LogInformation("Registered initial orchestration {OrchestrationName}.", definition.Name);
            }

            var lifecycle = await _orchestrationLifecycleRepository.GetByIdAsync(orchestration.Id, cancellationToken);
            if (lifecycle is null)
            {
                lifecycle = OrchestrationLifecycle.Create(orchestration.Id);
                await _orchestrationLifecycleRepository.AddAsync(lifecycle, cancellationToken);
            }

            foreach (var systemName in definition.Systems)
            {
                if (!systems.TryGetValue(systemName, out var system))
                {
                    throw new InvalidOperationException($"System '{systemName}' required by orchestration " + $"'{definition.Name}' was not seeded.");
                }

                var association = await _orchestrationSystemRepository.GetByOrchestrationAndSystemAsync(
                    orchestration.Id, system.Id, includeDeleted: true, cancellationToken);

                if (association is null)
                {
                    association = OrchestrationSystem.Create(orchestration.Id, system.Id);
                    await _orchestrationSystemRepository.AddAsync(association, cancellationToken);
                }
                else if (association.IsDeleted)
                {
                    association.Restore();
                    await _orchestrationSystemRepository.UpdateAsync(association, cancellationToken);
                }
            }
        }
    }
}