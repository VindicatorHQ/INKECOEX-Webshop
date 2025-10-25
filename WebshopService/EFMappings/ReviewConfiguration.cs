using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebshopService.Models;

namespace WebshopService.EFMappings;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Comment)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(r => r.ReviewerName)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(r => r.Rating)
            .IsRequired();
            
        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Guide)
            .WithMany(g => g.Reviews)
            .HasForeignKey(r => r.GuideId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(r => r.UserId)
            .HasMaxLength(450)
            .IsRequired(false);
    }
}