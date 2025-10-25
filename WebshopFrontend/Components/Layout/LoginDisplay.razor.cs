using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.Services;

namespace WebshopFrontend.Components.Layout;

public partial class LoginDisplay(IAuthAgent authAgent, ShoppingCartService cartService) : ComponentBase, IDisposable
{
    [Inject]
    private IThemeService themeService { get; set; } = default!; 
    
    private async Task Logout()
    {
        await authAgent.LogoutAsync();
        
        NavigationManager.NavigateTo("/", forceLoad: true);
    }
    
    private async Task ToggleTheme()
    {
        await themeService.ToggleThemeAsync();
    }
    
    protected override void OnInitialized()
    {
        cartService.OnChange += HandleCartChanged;
        themeService.OnThemeChanged += StateHasChanged;
    }

    private void HandleCartChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    private void NavigateToLoginPage()
    {
        NavigationManager.NavigateTo("/login");
    }
    
    private void NavigateToRegisterPage()
    {
        NavigationManager.NavigateTo("/register");
    }
    
    public void Dispose()
    {
        cartService.OnChange -= HandleCartChanged;
        themeService.OnThemeChanged -= StateHasChanged;
    }
}