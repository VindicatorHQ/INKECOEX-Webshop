using Microsoft.AspNetCore.Identity;
using WebshopService.Models;
using WebshopService.Repositories.Interface;
using WebshopService.Utils; // Nodig voor ToSlug()

namespace WebshopService.Data;

public class DbInitializer(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    // NIEUW: Injecteer de Guide Repository
    IGuideRepository guideRepository)
{

    public async Task InitializeAsync()
    {
        await EnsureRolesExistAsync();

        await EnsureUsersExistAsync();

        await SeedCategoriesAsync();

        await SeedProductsAsync();
        
        // NIEUW: Voeg de Guides toe
        await SeedGuidesAsync();
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

    private async Task SeedGuidesAsync()
    {
        if ((await guideRepository.GetAllAsync()).Any())
        {
            return;
        }

        var guides = new List<Guide>();

        var guide1Title = "Migreren van Windows naar Linux Pro: Een Stap-voor-Stap Gids";
        guides.Add(new Guide
        {
            Title = guide1Title,
            Slug = guide1Title.ToSlug(),
            Content = @"
                <h2>Welkom bij Linux Pro!</h2>
                <p>Overstappen naar Linux kan eng lijken, maar met deze gids wordt het een fluitje van een cent.</p>
                
                <h3>Stap 1: Back-up maken</h3>
                <p>Zorg ervoor dat al je belangrijke bestanden veilig zijn. Gebruik een externe harde schijf of een cloudopslagdienst.</p>
                
                <h3>Stap 2: De installatie USB creëren</h3>
                <p>Download de Linux Pro ISO en gebruik een tool als Rufus of BalenaEtcher om een opstartbare USB-stick te maken.</p>
                
                <h3>Stap 3: De installatie</h3>
                <p>Start je computer opnieuw op vanaf de USB-stick en volg de instructies op het scherm. Tip: kies voor een dual-boot installatie als je Windows nog wilt behouden.</p>
                
                <h4>Waarom overstappen?</h4>
                <ul>
                    <li>Superieure snelheid en stabiliteit.</li>
                    <li>Geen licentiekosten (open source).</li>
                    <li>Volledige controle over je systeem.</li>
                </ul>
            "
        });

        var guide2Title = "Verbeter je Project Workflow met Project Planner Premium";
        guides.Add(new Guide
        {
            Title = guide2Title,
            Slug = guide2Title.ToSlug(),
            Content = @"
                <h1>Agile Workflow Optimalisatie</h1>
                <p>Project Planner Premium is ontworpen om teams te helpen efficiënter te werken. Hier zijn de beste tips:</p>
                
                <h2>Board View instellen</h2>
                <p>Gebruik het <strong>Kanban Board</strong> om je taken te visualiseren. Maak kolommen voor 'To Do', 'In Progress', 'Testing' en 'Done'.</p>
                
                <h3>Tijdschattingen</h3>
                <p>Elke taak moet een duidelijke tijdschatting hebben. De Tool gebruikt deze data om automatische rapporten te genereren over de projectvoortgang.</p>
                
                <ol>
                    <li>Definieer de taak.</li>
                    <li>Wijs een teamlid toe.</li>
                    <li>Voer de geschatte tijd in (in uren).</li>
                    <li>Start de taak en de ingebouwde tijdregistratie tool gaat mee.</li>
                </ol>
                
                <p>Dit garandeert dat je nooit meer verrast wordt door budgetoverschrijdingen.</p>
            "
        });

        var guide3Title = "De Essentie van Cybersecurity: Bescherming met VPN en Antivirus";
        guides.Add(new Guide
        {
            Title = guide3Title,
            Slug = guide3Title.ToSlug(),
            Content = @"
                <h1>Bescherm Jezelf Online</h1>
                <p>Met de toename van cyberdreigingen is goede beveiligingssoftware essentieel.</p>
                
                <h2>Ultimate Secure VPN</h2>
                <p>Onze VPN versleutelt al je internetverkeer. Dit is cruciaal wanneer je gebruik maakt van openbare Wi-Fi netwerken. Het verbergt ook je IP-adres, wat je privacy verhoogt.</p>

                <h2>Malware Defender Pro</h2>
                <p>Naast een VPN heb je lokale bescherming nodig tegen virussen en ransomware. Malware Defender Pro gebruikt AI om nieuwe dreigingen te herkennen voordat ze schade kunnen aanrichten.</p>
                
                <p><strong>Checklist voor Optimale Beveiliging:</strong></p>
                <table class='table table-bordered dark-table'>
                    <thead>
                        <tr><th>Gebruik</th><th>Vereist</th></tr>
                    </thead>
                    <tbody>
                        <tr><td>Openbare Wi-Fi</td><td>VPN altijd AAN</td></tr>
                        <tr><td>Nieuwe software installeren</td><td>Antivirus scan vooraf</td></tr>
                        <tr><td>Wachtwoordbeheer</td><td>Gebruik Wachtwoord Manager Deluxe</td></tr>
                    </tbody>
                </table>
            "
        });
        
        var guide4Title = "Migratie van Windows 11 naar Arch Linux: De Ultieme Uitdaging";
        guides.Add(new Guide
        {
            Title = guide4Title,
            Slug = guide4Title.ToSlug(),
            Content = @"
                <h1>Arch Linux: De Doe-Het-Zelf Distributie</h1>
                <p>Arch Linux is een van de krachtigste en flexibelste Linux-distributies, maar staat bekend om zijn steile leercurve. Deze gids helpt je bij de overstap vanaf Windows 11.</p>
                
                <h2>Wat je moet weten (Voor- en Nadelen)</h2>
                
                <div class='row dark-container'>
                    <div class='col-md-6'>
                        <h3>Voordelen (Pros)</h3>
                        <ul>
                            <li><strong>Rolling Release:</strong> Altijd de nieuwste software zonder grote upgrades.</li>
                            <li><strong>Volledige Controle:</strong> Je bouwt je systeem van de grond af op, wat resulteert in een extreem lichte en snelle installatie.</li>
                            <li><strong>AUR (Arch User Repository):</strong> Toegang tot een onofficiële, maar enorme, verzameling softwarepakketten.</li>
                            <li><strong>Kennisopbouw:</strong> Je leert extreem veel over hoe een Linux-systeem werkt.</li>
                        </ul>
                    </div>
                    <div class='col-md-6'>
                        <h3>Nadelen (Cons)</h3>
                        <ul>
                            <li><strong>Steile Leercurve:</strong> Er is geen grafische installer; alles gebeurt via de commandoregel.</li>
                            <li><strong>Onderhoud:</strong> Meer hands-on beheer vereist (geen 'set-and-forget' systeem).</li>
                            <li><strong>Breukgevoeligheid:</strong> Door de rolling release kan een verkeerde update soms tot problemen leiden.</li>
                            <li><strong>Hardware Support:</strong> Soms meer moeite met gespecialiseerde drivers dan bij een OS als Windows.</li>
                        </ul>
                    </div>
                </div>
                
                <h2>Basisstappen voor Arch Installatie</h2>
                <ol>
                    <li>Bereid de USB-stick voor en start op.</li>
                    <li>Maak verbinding met internet (<code>wifi-menu</code> of bekabeld).</li>
                    <li>Partitioneer de schijf (<code>fdisk</code> of <code>cfdisk</code>).</li>
                    <li>Mount de bestandssystemen.</li>
                    <li>Installeer de basispakketten (<code>pacstrap /mnt base linux linux-firmware</code>).</li>
                    <li>Genereer de Fstab (<code>genfstab -U /mnt >> /mnt/etc/fstab</code>).</li>
                    <li>Chroot in de nieuwe installatie (<code>arch-chroot /mnt</code>).</li>
                    <li>Stel locale, tijdzone en root-wachtwoord in.</li>
                    <li>Installeer een bootloader (bijv. GRUB).</li>
                    <li>Installeer en configureer je gewenste Desktop Environment (DE).</li>
                </ol>
            "
        });
        
        foreach (var guide in guides)
        {
            await guideRepository.AddAsync(guide);
        }
    }
}
