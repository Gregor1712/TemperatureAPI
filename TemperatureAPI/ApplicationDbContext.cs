using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TemperatureAPI.Models;
using TemperatureAPI.Entities;

public class ApplicationDbContext(DbContextOptions options): IdentityDbContext<AppUser>(options)
{
    public DbSet<TemperatureHistory> TemperatureHistory { get; set; }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

// help:
// dotnet ef migrations add Initial
// dotnet ef database update
