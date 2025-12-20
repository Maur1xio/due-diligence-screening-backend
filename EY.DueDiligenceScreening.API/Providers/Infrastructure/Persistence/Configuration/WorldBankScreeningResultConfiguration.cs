using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.Providers.Infrastructure.Persistence.Configuration;

public class WorldBankScreeningResultConfiguration : IEntityTypeConfiguration<WorldBankScreeningResult>
{
    public void Configure(EntityTypeBuilder<WorldBankScreeningResult> builder)
    {
        builder.ToTable("WorldBankScreeningResult");

        builder.HasKey(r => r.WorldBankResultId);

        builder.Property(r => r.WorldBankResultId)
            .HasColumnName("world_bank_result_id")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.ScreeningHistoryId)
            .IsRequired()
            .HasColumnName("screening_history_id");

        builder.Property(r => r.FirmName)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("firm_name");

        builder.Property(r => r.Address)
            .HasMaxLength(500)
            .HasColumnName("address");

        builder.Property(r => r.Country)
            .HasMaxLength(100)
            .HasColumnName("country");

        builder.Property(r => r.FromDate)
            .HasMaxLength(50)
            .HasColumnName("from_date");

        builder.Property(r => r.ToDate)
            .HasMaxLength(50)
            .HasColumnName("to_date");

        builder.Property(r => r.Grounds)
            .HasMaxLength(1000)
            .HasColumnName("grounds");

        builder.HasIndex(r => r.ScreeningHistoryId)
            .HasDatabaseName("IX_WorldBankScreeningResult_HistoryId");
    }
}

