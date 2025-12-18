using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EY.DueDiligenceScreening.API.IAM.Domain.Model.Entities;

namespace EY.DueDiligenceScreening.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(u => u.UserID);

        builder.Property(u => u.UserID)
            .HasColumnName("user_id")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("email");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("password_hash");

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("full_name");

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_User_Email");
    }
}

