using Athena.Domain.Block.Registry.Entities;
using Athena.Domain.Block.Registry.ValueObjects;
using Athena.Domain.Company.Entities;
using Athena.Domain.Company.ValueObjects;
using Athena.Domain.Ecosystem.Registry.Entities;
using Athena.Domain.Ecosystem.Registry.ValueObjects;
using Athena.Domain.System.Management.Entities;
using Athena.Domain.System.Management.Enums;
using Athena.Domain.System.Registry.Entities;
using Athena.Domain.System.Registry.ValueObjects;
using Athena.Domain.Workload.Registry.Entities;
using Athena.Domain.Workload.Registry.ValueObjects;
using Athena.Infrastructure.Persistence;
using Athena.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using RegisteredSystem = Athena.Domain.System.Registry.Entities.System;

namespace Athena.UnitTests;

public sealed class SystemInstanceOperationPostgresTests
{
    [PostgreSqlFact]
    [Trait("Category", "PostgreSQL")]
    public async Task ClaimsAllOperationTypesAtomicallyAndRejectsStaleLeaseOwners()
    {
        await using var postgres = string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ATHENA_TEST_POSTGRES"))
            ? new PostgreSqlBuilder("postgres:16-alpine").Build()
            : null;

        var connectionString = Environment.GetEnvironmentVariable("ATHENA_TEST_POSTGRES");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            await postgres!.StartAsync();
            connectionString = postgres.GetConnectionString();
        }

        var options = new DbContextOptionsBuilder<AthenaDbContext>().UseNpgsql(connectionString).Options;
        var testRunId = Guid.NewGuid();

        await using (var context = new AthenaDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            await SeedRegistryAsync(context, 11, testRunId);

            var repository = new SystemInstanceOperationRepository(context);
            var operationTypes = Enum.GetValues<SystemInstanceOperationType>();
            for (var index = 0; index < operationTypes.Length; index++)
            {
                var instanceId = SystemInstanceId.Create(InstanceId(index, testRunId));
                await repository.AddAsync(SystemInstanceOperation.Create(instanceId, operationTypes[index]));
            }
        }

        var claimedTypes = new List<SystemInstanceOperationType>();
        for (var index = 0; index < Enum.GetValues<SystemInstanceOperationType>().Length; index++)
        {
            await using var context = new AthenaDbContext(options);
            var repository = new SystemInstanceOperationRepository(context);
            var claimed = await repository.ClaimNextAsync(
                $"type-worker-{index}",
                TimeSpan.FromMinutes(2),
                DateTime.UtcNow);
            Assert.NotNull(claimed);
            claimedTypes.Add(claimed.Type);
        }
        Assert.Equal(Enum.GetValues<SystemInstanceOperationType>().Order(), claimedTypes.Order());

        await using (var context = new AthenaDbContext(options))
        {
            var repository = new SystemInstanceOperationRepository(context);
            await repository.AddAsync(SystemInstanceOperation.Create(
                SystemInstanceId.Create(InstanceId(8, testRunId)), SystemInstanceOperationType.Start));
            await repository.AddAsync(SystemInstanceOperation.Create(
                SystemInstanceId.Create(InstanceId(9, testRunId)), SystemInstanceOperationType.Stop));
        }

        SystemInstanceOperation? firstClaim;
        SystemInstanceOperation? secondClaim;
        await using (var firstContext = new AthenaDbContext(options))
        await using (var secondContext = new AthenaDbContext(options))
        {
            var firstRepository = new SystemInstanceOperationRepository(firstContext);
            var secondRepository = new SystemInstanceOperationRepository(secondContext);
            var claims = await Task.WhenAll(
                firstRepository.ClaimNextAsync("concurrent-worker-1", TimeSpan.FromMinutes(2), DateTime.UtcNow),
                secondRepository.ClaimNextAsync("concurrent-worker-2", TimeSpan.FromMinutes(2), DateTime.UtcNow));
            firstClaim = claims[0];
            secondClaim = claims[1];
        }

        Assert.NotNull(firstClaim);
        Assert.NotNull(secondClaim);
        Assert.NotEqual(firstClaim.Id, secondClaim.Id);

        await using (var context = new AthenaDbContext(options))
        {
            var repository = new SystemInstanceOperationRepository(context);
            await repository.AddAsync(SystemInstanceOperation.Create(
                SystemInstanceId.Create(InstanceId(10, testRunId)), SystemInstanceOperationType.Restart));
        }

        await using (var staleContext = new AthenaDbContext(options))
        {
            var staleRepository = new SystemInstanceOperationRepository(staleContext);
            var stale = await staleRepository.ClaimNextAsync(
                "expired-worker",
                TimeSpan.FromSeconds(1),
                DateTime.UtcNow.AddMinutes(-2));
            Assert.NotNull(stale);
            stale.Start();
            Assert.False(await staleRepository.SaveIfLeaseOwnerAsync(stale, "expired-worker"));
        }

        await using (var recoveryContext = new AthenaDbContext(options))
        {
            var recoveryRepository = new SystemInstanceOperationRepository(recoveryContext);
            var recovered = await recoveryRepository.ClaimNextAsync(
                "recovery-worker",
                TimeSpan.FromMinutes(2),
                DateTime.UtcNow);
            Assert.NotNull(recovered);
            Assert.Equal(InstanceId(10, testRunId), recovered.SystemInstanceId.Value);
        }
    }

    private static Guid InstanceId(int index, Guid testRunId)
    {
        var runId = testRunId.ToString("N");
        return Guid.Parse($"{runId[..8]}-{runId[8..12]}-{runId[12..16]}-{runId[16..20]}-{index + 1:000000000000}");
    }

    private static async Task SeedRegistryAsync(AthenaDbContext context, int instanceCount, Guid testRunId)
    {
        var suffix = testRunId.ToString("N");
        var company = Company.Create(
            CompanyId.Create(Guid.NewGuid()),
            CompanyName.Create($"Test company {suffix}"),
            LegalName.Create($"Test company legal name {suffix}"),
            CountryCode.Create("IT"));
        context.Companies.Add(company);
        await context.SaveChangesAsync();
        var companyDatabaseId = context.Entry(company).Property<int>("id").CurrentValue;

        var ecosystem = Ecosystem.Create(
            EcosystemId.Create(Guid.NewGuid()),
            company.Id,
            EcosystemName.Create($"Test ecosystem {suffix}"));
        context.Ecosystems.Add(ecosystem);
        context.Entry(ecosystem).Property<int>("company_database_id").CurrentValue = companyDatabaseId;
        await context.SaveChangesAsync();
        var ecosystemDatabaseId = context.Entry(ecosystem).Property<int>("id").CurrentValue;

        var block = Block.Create(
            BlockId.Create(Guid.NewGuid()),
            ecosystem.Id,
            BlockName.Create($"Test block {suffix}"));
        context.Blocks.Add(block);
        context.Entry(block).Property<int>("ecosystem_database_id").CurrentValue = ecosystemDatabaseId;
        await context.SaveChangesAsync();
        var blockDatabaseId = context.Entry(block).Property<int>("id").CurrentValue;

        var workload = Workload.Create(
            WorkloadId.Create(Guid.NewGuid()),
            block.Id,
            WorkloadName.Create($"Test workload {suffix}"));
        context.Workloads.Add(workload);
        context.Entry(workload).Property<int>("block_database_id").CurrentValue = blockDatabaseId;
        await context.SaveChangesAsync();
        var workloadDatabaseId = context.Entry(workload).Property<int>("id").CurrentValue;

        var system = RegisteredSystem.Create(
            SystemId.Create(Guid.NewGuid()),
            SystemName.Create($"Test system {suffix}"));
        context.Systems.Add(system);
        await context.SaveChangesAsync();
        var systemDatabaseId = context.Entry(system).Property<int>("id").CurrentValue;

        for (var index = 0; index < instanceCount; index++)
        {
            var instance = SystemInstance.Create(
                SystemInstanceId.Create(InstanceId(index, testRunId)),
                system.Id,
                workload.Id,
                SystemInstanceName.Create($"instance-{index}-{suffix}"));
            context.SystemInstances.Add(instance);
            context.Entry(instance).Property<int>("system_database_id").CurrentValue = systemDatabaseId;
            context.Entry(instance).Property<int>("workload_database_id").CurrentValue = workloadDatabaseId;
        }

        await context.SaveChangesAsync();
    }

    public sealed class PostgreSqlFactAttribute : FactAttribute
    {
        public PostgreSqlFactAttribute()
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ATHENA_TEST_POSTGRES"))
                && !string.Equals(Environment.GetEnvironmentVariable("ATHENA_RUN_POSTGRES_TESTS"), "true", StringComparison.OrdinalIgnoreCase))
            {
                Skip = "Set ATHENA_RUN_POSTGRES_TESTS=true to run the Docker-backed PostgreSQL integration test.";
            }
        }
    }
}
