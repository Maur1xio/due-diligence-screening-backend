using Microsoft.EntityFrameworkCore;
using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}

