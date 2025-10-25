using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebshopService.Models;

namespace WebshopService.EFMappings;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.HasKey(sa => sa.Id);

        builder.HasOne(sa => sa.Order)
            .WithOne(o => o.ShippingAddress)
            .HasForeignKey<ShippingAddress>(sa => sa.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    
        builder.Property(sa => sa.FullName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(sa => sa.Street)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(sa => sa.HouseNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(sa => sa.ZipCode)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(sa => sa.City)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(sa => sa.Country)
            .HasMaxLength(100)
            .IsRequired();
    }
}