using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using WebshopFrontend.Agents;
using WebshopFrontend.Agents.Implementation;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.Components;
using WebshopFrontend.Providers;
using WebshopFrontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    })
    .AddIdentityCookies();

builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

var agentUrl = 
    Environment.GetEnvironmentVariable("AgentUrl") ??
    builder.Configuration["AgentUrl"];

builder.Services.AddSingleton(new AgentUrl<AuthAgent>(agentUrl));
builder.Services.AddTransient<IAuthAgent, AuthAgent>();

builder.Services.AddSingleton(new AgentUrl<CategoryAgent>(agentUrl));
builder.Services.AddTransient<ICategoryAgent, CategoryAgent>();

builder.Services.AddSingleton(new AgentUrl<OrderAgent>(agentUrl));
builder.Services.AddTransient<IOrderAgent, OrderAgent>();

builder.Services.AddSingleton(new AgentUrl<ProductAgent>(agentUrl));
builder.Services.AddTransient<IProductAgent, ProductAgent>();

builder.Services.AddSingleton(new AgentUrl<ShoppingCartAgent>(agentUrl));
builder.Services.AddTransient<IShoppingCartAgent, ShoppingCartAgent>();

builder.Services.AddSingleton(new AgentUrl<UserAgent>(agentUrl));
builder.Services.AddTransient<IUserAgent, UserAgent>();

builder.Services.AddSingleton(new AgentUrl<GuideAgent>(agentUrl));
builder.Services.AddTransient<IGuideAgent, GuideAgent>();

builder.Services.AddSingleton(new AgentUrl<ReviewAgent>(agentUrl));
builder.Services.AddTransient<IReviewAgent, ReviewAgent>();

builder.Services.AddSingleton<IDebounceService, DebounceService>();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddSingleton<ShoppingCartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();