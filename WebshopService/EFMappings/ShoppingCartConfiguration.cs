using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebshopService.Models;

namespace WebshopService.EFMappings;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(sc => sc.Id);

        builder.HasIndex(sc => sc.UserId)
            .IsUnique();

        builder.HasOne(sc => sc.User)
            .WithOne()
            .HasForeignKey<ShoppingCart>(sc => sc.UserId)
            .IsRequired();
    }
}