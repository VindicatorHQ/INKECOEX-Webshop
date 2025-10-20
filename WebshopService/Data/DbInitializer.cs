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
        var prodCategory = (await categoryRepository.GetBySlugAsync("productivity")).Id;
        var gameCategory = (await categoryRepository.GetBySlugAsync("games")).Id;

        var products = new List<Product>
        {
            new Product
            {
                Name = "Linux Pro OS Licentie",
                Description = "Een stabiele, geoptimaliseerde Linux distributie voor professionals en overstappers.",
                Price = 49.99m, StockQuantity = 100,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = osCategory } }
            },
            new Product
            {
                Name = "Windows Workstation 12 Pro",
                Description = "De nieuwste Windows versie, geoptimaliseerd voor enterprise en zware werkbelasting.",
                Price = 249.99m, StockQuantity = 50,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = osCategory } }
            },
            new Product
            {
                Name = "macOS Ventura Upgrade Kit",
                Description = "Digitale licentie voor de meest recente macOS-upgrade.",
                Price = 129.99m, StockQuantity = 75,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = osCategory } }
            },
            new Product
            {
                Name = "Embedded RTOS Licentie",
                Description = "Real-Time Operating System voor IoT en embedded apparaten.",
                Price = 89.99m, StockQuantity = 30,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = osCategory } }
            },
            new Product
            {
                Name = "FreeBSD Server Edition",
                Description = "Robuust en veilig besturingssysteem, ideaal voor webservers en netwerken.",
                Price = 0.00m,
                StockQuantity = 999,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = osCategory } }
            },

            new Product
            {
                Name = "DevTools Suite 2024",
                Description = "All-in-one pakket voor webontwikkelaars: IDE, database tools en deployment scripts.",
                Price = 199.00m, StockQuantity = 50,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = devCategory } }
            },
            new Product
            {
                Name = "Python Data Science Toolkit",
                Description = "Bundel van Jupyter, Pandas en Scikit-learn met premium support.",
                Price = 49.95m, StockQuantity = 120,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = devCategory } }
            },
            new Product
            {
                Name = "Unity Pro Abonnement (Jaar)",
                Description = "Volledige jaarlicentie voor de Unity game-engine.",
                Price = 1499.00m, StockQuantity = 20,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = devCategory } }
            },
            new Product
            {
                Name = "Docker Enterprise License",
                Description = "Licentie voor containerisatie op grote schaal met security updates.",
                Price = 299.99m, StockQuantity = 45,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = devCategory } }
            },
            new Product
            {
                Name = "VS Code Extensions Pack",
                Description = "Set van essentiële VS Code extensies voor frontend ontwikkeling.",
                Price = 9.99m, StockQuantity = 300,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = devCategory } }
            },
            
            new Product
            {
                Name = "Ultimate Secure VPN",
                Description = "Premium VPN-dienst met nul-log beleid en 50+ locaties. Perfect voor privacy.",
                Price = 7.99m, StockQuantity = 500,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = secCategory } }
            },
            new Product
            {
                Name = "Malware Defender Pro (3 Apparaten)",
                Description = "AI-gedreven antivirus en malware scanner voor Windows, Mac en Android.",
                Price = 39.99m, StockQuantity = 250,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = secCategory } }
            },
            new Product
            {
                Name = "Wachtwoord Manager Deluxe",
                Description = "Versleutelde, cross-platform wachtwoordmanager met biometrische authenticatie.",
                Price = 19.95m, StockQuantity = 400,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = secCategory } }
            },
            new Product
            {
                Name = "Dark Web Monitoring Service",
                Description = "Jaarabonnement voor het monitoren van gelekte persoonsgegevens op het dark web.",
                Price = 59.00m, StockQuantity = 100,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = secCategory } }
            },
            new Product
            {
                Name = "Firewall Pro 2025",
                Description = "Geavanceerde netwerk firewall software voor MKB en thuisgebruik.",
                Price = 99.99m, StockQuantity = 60,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = secCategory } }
            },

            new Product
            {
                Name = "Office Suite Professional",
                Description = "Volledig pakket met Word, Excel, PowerPoint en Outlook.",
                Price = 149.99m, StockQuantity = 200,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = prodCategory } }
            },
            new Product
            {
                Name = "Project Planner Premium",
                Description = "Software voor Agile projectmanagement, planning en teamcoördinatie.",
                Price = 29.99m, StockQuantity = 150,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = prodCategory } }
            },
            new Product
            {
                Name = "Notities App Plus",
                Description = "Cloud-gebaseerde notitie-app met handschriftherkenning en tags.",
                Price = 4.99m, StockQuantity = 800,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = prodCategory } }
            },
            new Product
            {
                Name = "Tijdregistratie Tool",
                Description = "Automatische urenregistratie en facturatie software.",
                Price = 19.99m, StockQuantity = 100,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = prodCategory } }
            },
            new Product
            {
                Name = "PDF Editor Pro",
                Description = "Uitgebreide tool voor het bewerken, converteren en beveiligen van PDF-documenten.",
                Price = 79.99m, StockQuantity = 110,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = prodCategory } }
            },
            
            new Product
            {
                Name = "Cyberpunk Dreams (Limited Edition)",
                Description = "Gelimiteerde licentie voor een unieke indie-game met hoge user reviews.",
                Price = 34.50m, StockQuantity = 25,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = gameCategory } }
            },
            new Product
            {
                Name = "Space Explorer Alpha Key",
                Description = "Zeer zeldzame vroege toegangssleutel tot een nieuwe ruimte-simulatie game.",
                Price = 75.00m, StockQuantity = 10,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = gameCategory } }
            },
            new Product
            {
                Name = "Retro Platformer Collection",
                Description = "Bundel van 10 klassieke 8-bit platformers, exclusief digitaal.",
                Price = 19.99m, StockQuantity = 50,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = gameCategory } }
            },
            new Product
            {
                Name = "Mystery Indie Box (Season 3)",
                Description = "Een verrassingspakket met 3 onbekende indie-games.",
                Price = 15.00m, StockQuantity = 40,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = gameCategory } }
            },
            new Product
            {
                Name = "Fantasy RPG: The Lost Realm",
                Description = "Collector's Edition van een epische fantasy RPG.",
                Price = 49.99m, StockQuantity = 35,
                ProductCategories = new List<ProductCategory> { new ProductCategory { CategoryId = gameCategory } }
            }
        };

        foreach (var product in products)
        {
            await productRepository.CreateAsync(product);
        }
    }
}
