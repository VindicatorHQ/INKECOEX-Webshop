using Microsoft.AspNetCore.Identity;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Data;

public class DbInitializer(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IProductRepository productRepository,
    ICategoryRepository categoryRepository)
{

    public async Task InitializeAsync()
    {
        await EnsureRolesExistAsync();

        await EnsureUsersExistAsync();

        await SeedCategoriesAsync();

        await SeedProductsAsync();
    }

    private async Task EnsureRolesExistAsync()
    {
        string[] roleNames = { "Admin", "Consumer" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private async Task EnsureUsersExistAsync()
    {
        var adminEmail = "admin@webshop.nl";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "AdminWachtwoord1!");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        var customerEmail = "customer@webshop.nl";
        if (await userManager.FindByEmailAsync(customerEmail) == null)
        {
            var customerUser = new IdentityUser
            {
                UserName = customerEmail,
                Email = customerEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var result = await userManager.CreateAsync(customerUser, "CustomerWachtwoord1!");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(customerUser, "Consumer");
            }
        }
    }

    private async Task SeedCategoriesAsync()
    {
        if ((await categoryRepository.GetAllAsync()).Any())
        {
            return;
        }

        var categories = new List<Category>
        {
            new Category { Name = "Besturingssystemen", Slug = "os" },
            new Category { Name = "Ontwikkeling", Slug = "development" },
            new Category { Name = "Beveiliging & VPN", Slug = "security" },
            new Category { Name = "Productiviteit", Slug = "productivity" },
            new Category { Name = "Gelimiteerde Games", Slug = "games" }
        };

        foreach (var cat in categories)
        {
            await categoryRepository.AddAsync(cat);
        }
    }

    private async Task SeedProductsAsync()
    {
        if ((await productRepository.GetAllAsync()).Any())
        {
            return;
        }

        var osCategory = (await categoryRepository.GetBySlugAsync("os")).Id;
        var devCategory = (await categoryRepository.GetBySlugAsync("development")).Id;
        var secCategory = (await categoryRepository.GetBySlugAsync("security")).Id;
        var gameCategory = (await categoryRepository.GetBySlugAsync("games")).Id;

        var products = new List<Product>
        {
            new Product
            {
                Name = "Linux Pro OS Licentie",
                Description = "Een stabiele, geoptimaliseerde Linux distributie voor professionals en overstappers.",
                Price = 49.99m,
                StockQuantity = 100,
                ProductCategories = new List<ProductCategory>
                {
                    new ProductCategory { CategoryId = osCategory }
                }
            },
            new Product
            {
                Name = "Ultimate Secure VPN",
                Description = "Premium VPN-dienst met nul-log beleid en 50+ locaties. Perfect voor privacy.",
                Price = 7.99m,
                StockQuantity = 500,
                ProductCategories = new List<ProductCategory>
                {
                    new ProductCategory { CategoryId = secCategory }
                }
            },
            new Product
            {
                Name = "DevTools Suite 2024",
                Description = "All-in-one pakket voor webontwikkelaars: IDE, database tools en deployment scripts.",
                Price = 199.00m,
                StockQuantity = 50,
                ProductCategories = new List<ProductCategory>
                {
                    new ProductCategory { CategoryId = devCategory }
                }
            },
            new Product
            {
                Name = "Cyberpunk Dreams (Limited Edition)",
                Description = "Gelimiteerde licentie voor een unieke indie-game met hoge user reviews.",
                Price = 34.50m,
                StockQuantity = 25,
                ProductCategories = new List<ProductCategory>
                {
                    new ProductCategory { CategoryId = gameCategory }
                }
            }
        };

        foreach (var product in products)
        {
            await productRepository.CreateAsync(product);
        }
    }
}