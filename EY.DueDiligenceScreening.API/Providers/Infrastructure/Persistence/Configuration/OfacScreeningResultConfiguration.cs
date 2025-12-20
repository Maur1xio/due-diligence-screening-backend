using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.Providers.Infrastructure.Persistence.Configuration;

public class OfacScreeningResultConfiguration : IEntityTypeConfiguration<OfacScreeningResult>
{
    public void Configure(EntityTypeBuilder<OfacScreeningResult> builder)
    {
        builder.ToTable("OfacScreeningResult");

        builder.HasKey(r => r.OfacResultId);

        builder.Property(r => r.OfacResultId)
            .HasColumnName("ofac_result_id")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.ScreeningHistoryId)
            .IsRequired()
            .HasColumnName("screening_history_id");

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("name");

        builder.Property(r => r.Address)
            .HasMaxLength(500)
            .HasColumnName("address");

        builder.Property(r => r.Type)
            .HasMaxLength(100)
            .HasColumnName("type");

        builder.Property(r => r.Programs)
            .HasMaxLength(500)
            .HasColumnName("programs");

        builder.Property(r => r.List)
            .HasMaxLength(100)
            .HasColumnName("list");

        builder.Property(r => r.Score)
            .IsRequired()
            .HasColumnName("score");

        builder.Property(r => r.DetailsUrl)
            .HasMaxLength(1000)
            .HasColumnName("details_url");

        builder.HasIndex(r => r.ScreeningHistoryId)
            .HasDatabaseName("IX_OfacScreeningResult_HistoryId");
    }
}

