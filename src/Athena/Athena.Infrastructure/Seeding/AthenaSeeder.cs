using Microsoft.Extensions.Logging;
using Athena.Domain.Orchestration.Management.Entities;
using Athena.Domain.Orchestration.Management.Repositories;
using Athena.Domain.Orchestration.Registry.Entities;
using Athena.Domain.Orchestration.Registry.Repositories;
using Athena.Domain.Orchestration.Registry.ValueObjects;
using Athena.Domain.System.Registry.Repositories;
using Athena.Domain.System.Registry.ValueObjects;
using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;

namespace Athena.Infrastructure.Seeding;

public sealed class AthenaSeeder(
    ISystemRepository systemRepository,
    IOrchestrationRepository orchestrationRepository,
    IOrchestrationLifecycleRepository orchestrationLifecycleRepository,
    ILogger<AthenaSeeder> logger)
{
    private readonly ISystemRepository _systemRepository = systemRepository;
    private readonly IOrchestrationRepository _orchestrationRepository = orchestrationRepository;
    private readonly IOrchestrationLifecycleRepository _orchestrationLifecycleRepository = orchestrationLifecycleRepository;
    private readonly ILogger<AthenaSeeder> _logger = logger;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Athena database seeding.");
        await SeedSystemsAsync(cancellationToken);
        await SeedOrchestrationsAsync(cancellationToken);
        _logger.LogInformation("Athena database seeding completed.");
    }

    private async Task SeedSystemsAsync(CancellationToken cancellationToken)
    {
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
        }
    }

    private async Task SeedOrchestrationsAsync(CancellationToken cancellationToken)
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

        }
    }
}