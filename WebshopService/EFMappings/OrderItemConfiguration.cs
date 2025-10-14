using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebshopService.Models;

namespace WebshopService.EFMappings;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(od => od.Id);

        builder.Property(od => od.Quantity)
            .IsRequired();

        builder.Property(od => od.PriceAtOrder)
            .HasColumnType("numeric(18, 2)")
            .IsRequired();

        builder.HasOne(od => od.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(od => od.OrderId);

        builder.HasOne(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.ProductId);
    }
}