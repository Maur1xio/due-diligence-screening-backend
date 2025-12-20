using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EY.DueDiligenceScreening.API.Providers.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.Providers.Infrastructure.Persistence.Configuration;

public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable("Provider");

        builder.HasKey(p => p.ProviderId);

        builder.Property(p => p.ProviderId)
            .HasColumnName("provider_id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.UserId)
            .IsRequired()
            .HasColumnName("user_id");

        builder.Property(p => p.BusinessName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("business_name");

        builder.Property(p => p.TradeName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("trade_name");

        builder.Property(p => p.TaxId)
            .IsRequired()
            .HasMaxLength(11)
            .HasColumnName("tax_id");

        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("phone_number");

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("email");

        builder.Property(p => p.Website)
            .HasMaxLength(500)
            .HasColumnName("website");

        builder.Property(p => p.PhysicalAddress)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("physical_address");

        builder.Property(p => p.Country)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("country");

        builder.Property(p => p.AnnualBillingUsd)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("annual_billing_usd");

        builder.Property(p => p.LastEditedAt)
            .IsRequired()
            .HasColumnName("last_edited_at");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("IX_Provider_UserId");

        builder.HasOne(p => p.ScreeningHistory)
            .WithOne(h => h.Provider)
            .HasForeignKey<ScreeningHistory>(h => h.ProviderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

