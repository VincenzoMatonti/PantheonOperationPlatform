using Athena.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Athena.UnitTests;

public sealed class AthenaInfrastructureModelTests
{
    [Fact]
    public void Model_MapsSystemInstanceProcessesWithoutLegacyRuntimeEntity()
    {
        var options = new DbContextOptionsBuilder<AthenaDbContext>()
            .UseNpgsql("Host=localhost;Database=metadata;Username=metadata;Password=metadata")
            .Options;
        using var context = new AthenaDbContext(options);

        var entityNames = context.Model.GetEntityTypes()
            .Select(entity => entity.ClrType.Name)
            .ToArray();

        Assert.Contains("SystemInstance", entityNames);
        Assert.Contains("SystemInstanceOperation", entityNames);
        Assert.DoesNotContain("SystemRuntimeInstance", entityNames);
    }
}
