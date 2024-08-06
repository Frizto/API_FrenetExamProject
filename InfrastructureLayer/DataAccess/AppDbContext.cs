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
}

