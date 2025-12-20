using Microsoft.EntityFrameworkCore;
using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;
using EY.DueDiligenceScreening.API.Providers.Infrastructure.Persistence.Configuration;
using EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // IAM
    public DbSet<User> Users { get; set; }

    // Providers
    public DbSet<Provider> Providers { get; set; }
    public DbSet<ScreeningHistory> ScreeningHistories { get; set; }
    public DbSet<OfacScreeningResult> OfacScreeningResults { get; set; }
    public DbSet<OffshoreLeaksScreeningResult> OffshoreLeaksScreeningResults { get; set; }
    public DbSet<WorldBankScreeningResult> WorldBankScreeningResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // IAM
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        // Providers
        modelBuilder.ApplyConfiguration(new ProviderConfiguration());
        modelBuilder.ApplyConfiguration(new ScreeningHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new OfacScreeningResultConfiguration());
        modelBuilder.ApplyConfiguration(new OffshoreLeaksScreeningResultConfiguration());
        modelBuilder.ApplyConfiguration(new WorldBankScreeningResultConfiguration());
    }
}

