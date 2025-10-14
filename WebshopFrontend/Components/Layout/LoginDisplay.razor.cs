using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.Providers;
using WebshopFrontend.Services;

namespace WebshopFrontend.Components.Layout;

public partial class LoginDisplay(IAuthAgent authAgent, ShoppingCartService cartService) : ComponentBase, IDisposable
{
    private async Task Logout()
    {
        await authAgent.LogoutAsync();
        
        NavigationManager.NavigateTo("/", forceLoad: true);
    }
    
    protected override void OnInitialized()
    {
        cartService.OnChange += HandleCartChanged;
        
        // Optioneel: probeer de cart bij het opstarten op te halen als de gebruiker ingelogd is
        // Dit is beter te doen in de MainLayout om een race condition te voorkomen.
    }

    private void HandleCartChanged()
    {
        InvokeAsync(StateHasChanged);
    }
    
    public void Dispose()
    {
        cartService.OnChange -= HandleCartChanged;
    }
}