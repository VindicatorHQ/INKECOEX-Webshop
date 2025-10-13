using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebshopService.Constants;
using WebshopService.Models;

namespace WebshopService.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        WebshopDbContext context, 
        UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        await context.Database.MigrateAsync(); 

        await SeedRolesAsync(roleManager);

        await SeedAdminUserAsync(userManager);
        
        await SeedProductsAndCategoriesAsync(context);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = [Roles.Admin, Roles.Consumer];

        foreach (var roleName in roleNames)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager)
    {
        if (await userManager.FindByNameAsync("admin@webshop.nl") == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = "admin@webshop.nl",
                Email = "admin@webshop.nl",
                EmailConfirmed = true 
            };

            var result = await userManager.CreateAsync(adminUser, "SecureAdmin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
        }
    }

    private static async Task SeedProductsAndCategoriesAsync(WebshopDbContext context)
    {
        if (!context.Products.Any())
        {
            var electronics = new Category { Name = "Electronics" };
            var books = new Category { Name = "Books" };
            
            context.Categories.AddRange(electronics, books);

            var product1 = new Product { Name = "Laptop X1", Description = "High-end laptop", Price = 1200.00M, StockQuantity = 15 };
            var product2 = new Product { Name = "Fantasy Roman", Description = "Epic fantasy book", Price = 19.95M, StockQuantity = 50 };

            context.Products.AddRange(product1, product2);
            
            context.ProductCategories.AddRange(
                new ProductCategory { Product = product1, Category = electronics },
                new ProductCategory { Product = product2, Category = books }
            );

            await context.SaveChangesAsync();
        }
    }
}