using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Polly;
using WebshopService.Data;
using WebshopService.Repositories;
using WebshopService.Repositories.Implementation;
using WebshopService.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

var policy = Policy
    .Handle<Exception>()
    .WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(3));

var connectionString = 
    Environment.GetEnvironmentVariable("INKECOEX_Webshop") ??
    builder.Configuration.GetConnectionString("INKECOEX_Webshop");

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<WebshopDbContext>(o=> o.UseNpgsql(connectionString));

builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<WebshopDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.MaxDepth = 256;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

await policy.ExecuteAsync(async () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        
        var context = services.GetRequiredService<WebshopDbContext>();
        
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var productRepository = services.GetRequiredService<IProductRepository>();
        var categoryRepository = services.GetRequiredService<ICategoryRepository>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync(); 

        var initializer = new DbInitializer(userManager, roleManager, productRepository, categoryRepository);
        
        await initializer.InitializeAsync();
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(swaggerOptions => { swaggerOptions.SwaggerEndpoint("/openapi/v1.json", "v1");});
    
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();