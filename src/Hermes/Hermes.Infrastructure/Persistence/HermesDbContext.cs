using Microsoft.EntityFrameworkCore;

namespace Hermes.Infrastructure.Persistence;

public class HermesDbContext(DbContextOptions<HermesDbContext> options) : DbContext(options)
{
}