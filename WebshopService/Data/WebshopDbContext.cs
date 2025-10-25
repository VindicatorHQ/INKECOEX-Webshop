using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebshopService.EFMappings;
using WebshopService.Models;

namespace WebshopService.Data;

public class WebshopDbContext(DbContextOptions<WebshopDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; } = default!;
    public DbSet<Guide> Guides { get; set; }
    public DbSet<ShippingAddress> ShippingAddresses { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new CartItemConfiguration());
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new OrderConfiguration());
        builder.ApplyConfiguration(new OrderItemConfiguration());
        builder.ApplyConfiguration(new ProductCategoryConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new ShoppingCartConfiguration());
        builder.ApplyConfiguration(new UserProfileConfiguration());
        builder.ApplyConfiguration(new GuideConfiguration());
        builder.ApplyConfiguration(new ShippingAddressConfiguration());
        builder.ApplyConfiguration(new ReviewConfiguration());
    }
}