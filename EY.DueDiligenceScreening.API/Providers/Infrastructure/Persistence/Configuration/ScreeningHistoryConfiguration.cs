using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.Providers.Infrastructure.Persistence.Configuration;

public class ScreeningHistoryConfiguration : IEntityTypeConfiguration<ScreeningHistory>
{
    public void Configure(EntityTypeBuilder<ScreeningHistory> builder)
    {
        builder.ToTable("ScreeningHistory");

        builder.HasKey(h => h.ScreeningHistoryId);

        builder.Property(h => h.ScreeningHistoryId)
            .HasColumnName("screening_history_id")
            .ValueGeneratedOnAdd();

        builder.Property(h => h.ProviderId)
            .IsRequired()
            .HasColumnName("provider_id");

        builder.Property(h => h.ScreenedAt)
            .IsRequired()
            .HasColumnName("screened_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(h => h.ProviderId)
            .IsUnique()
            .HasDatabaseName("IX_ScreeningHistory_ProviderId");

        builder.HasMany(h => h.OfacResults)
            .WithOne(r => r.ScreeningHistory)
            .HasForeignKey(r => r.ScreeningHistoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(h => h.OffshoreLeaksResults)
            .WithOne(r => r.ScreeningHistory)
            .HasForeignKey(r => r.ScreeningHistoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(h => h.WorldBankResults)
            .WithOne(r => r.ScreeningHistory)
            .HasForeignKey(r => r.ScreeningHistoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

