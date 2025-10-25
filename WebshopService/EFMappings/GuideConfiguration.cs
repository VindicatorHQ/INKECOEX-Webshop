using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebshopService.Models;

namespace WebshopService.EFMappings;

public class GuideConfiguration : IEntityTypeConfiguration<Guide>
{
    public void Configure(EntityTypeBuilder<Guide> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(g => g.Content)
            .IsRequired();

        builder.Property(g => g.Slug)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(g => g.Slug)
            .IsUnique();

        builder.Property(g => g.CreatedAt).IsRequired();
        builder.Property(g => g.UpdatedAt).IsRequired();
    }
}