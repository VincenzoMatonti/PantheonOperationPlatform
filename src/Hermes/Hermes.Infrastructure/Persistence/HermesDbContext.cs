using Hermes.Domain.Clients.Entities;
using Hermes.Domain.Endpoints.Entities;
using Hermes.Domain.Executions.Entities;
using Hermes.Domain.Operations.Entities;
using Hermes.Domain.Packages.Entities;
using Hermes.Domain.Routes.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Persistence;

public class HermesDbContext(DbContextOptions<HermesDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ClientOperation> ClientOperations => Set<ClientOperation>();
    public DbSet<Endpoint> Endpoints => Set<Endpoint>();
    public DbSet<EndpointOperation> EndpointOperations => Set<EndpointOperation>();
    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<OperationType> OperationTypes => Set<OperationType>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<Execution> Executions => Set<Execution>();
    public DbSet<ExecutionStep> ExecutionSteps => Set<ExecutionStep>();
    public DbSet<ExecutionStepRoute> ExecutionStepRoutes => Set<ExecutionStepRoute>();
    public DbSet<ExecutionStepType> ExecutionStepTypes => Set<ExecutionStepType>();
    public DbSet<ExecutionType> ExecutionTypes => Set<ExecutionType>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<Data> Data => Set<Data>();
    public DbSet<Header> Headers => Set<Header>();
    public DbSet<Metadata> Metadata => Set<Metadata>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(HermesDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}