using InfrastructureLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.DataAccess;

public partial class LogsDbContext(DbContextOptions<LogsDbContext> options) : DbContext
{
    public virtual DbSet<Nlog> Nlogs { get; set; }

    public virtual DbSet<NlogCreate> NlogCreates { get; set; }

    public virtual DbSet<NlogDelete> NlogDeletes { get; set; }

    public virtual DbSet<NlogUpdate> NlogUpdates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_FRENETEXAMLOGS_DEV");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("DB_CONNECTION_STRING is not set");
        }
        optionsBuilder.UseSqlServer(connectionString);
    }
}
