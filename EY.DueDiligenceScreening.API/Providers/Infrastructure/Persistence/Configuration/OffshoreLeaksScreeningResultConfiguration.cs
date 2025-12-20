using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.Providers.Infrastructure.Persistence.Configuration;

public class OffshoreLeaksScreeningResultConfiguration : IEntityTypeConfiguration<OffshoreLeaksScreeningResult>
{
    public void Configure(EntityTypeBuilder<OffshoreLeaksScreeningResult> builder)
    {
        builder.ToTable("OffshoreLeaksScreeningResult");

        builder.HasKey(r => r.OffshoreLeaksResultId);

        builder.Property(r => r.OffshoreLeaksResultId)
            .HasColumnName("offshore_leaks_result_id")
            .ValueGeneratedOnAdd();

        builder.Property(r => r.ScreeningHistoryId)
            .IsRequired()
            .HasColumnName("screening_history_id");

        builder.Property(r => r.EntityName)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("entity_name");

        builder.Property(r => r.EntityNodeUrl)
            .HasMaxLength(1000)
            .HasColumnName("entity_node_url");

        builder.Property(r => r.Jurisdiction)
            .HasMaxLength(200)
            .HasColumnName("jurisdiction");

        builder.Property(r => r.LinkedTo)
            .HasMaxLength(500)
            .HasColumnName("linked_to");

        builder.Property(r => r.DataSource)
            .HasMaxLength(200)
            .HasColumnName("data_source");

        builder.Property(r => r.DataSourceUrl)
            .HasMaxLength(1000)
            .HasColumnName("data_source_url");

        builder.HasIndex(r => r.ScreeningHistoryId)
            .HasDatabaseName("IX_OffshoreLeaksScreeningResult_HistoryId");
    }
}

