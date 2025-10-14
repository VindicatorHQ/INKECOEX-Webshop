using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using WebshopFrontend.Agents;
using WebshopFrontend.Components;
using WebshopFrontend.Providers;

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

builder.Services.AddSingleton(new AgentUrl<ProductAgent>(agentUrl));
builder.Services.AddTransient<IProductAgent, ProductAgent>();

builder.Services.AddSingleton(new AgentUrl<AuthAgent>(agentUrl));
builder.Services.AddTransient<IAuthAgent, AuthAgent>();

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