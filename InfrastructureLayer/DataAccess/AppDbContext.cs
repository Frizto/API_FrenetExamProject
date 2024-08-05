using ApplicationLayer.Extensions;
using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, IdentityRole, string>(options)
{
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Shipment> Shipments { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        string connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_FRENETEXAM_DEV")
    //            ?? throw new InvalidOperationException("DB_CONNECTION_STRING is not set");

    //        optionsBuilder.UseSqlServer(connectionString);
    //    }
    //}
}

