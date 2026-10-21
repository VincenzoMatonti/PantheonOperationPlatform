using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Athena.Infrastructure.Persistence;

public class AthenaDbContextFactory : IDesignTimeDbContextFactory<AthenaDbContext>
{
    public AthenaDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ATHENA_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("The ATHENA_CONNECTION_STRING environment variable is not set.");
        
        var optionsBuilder = new DbContextOptionsBuilder<AthenaDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new AthenaDbContext(optionsBuilder.Options);
    }
}
