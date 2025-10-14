using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents;
using WebshopFrontend.Providers;

namespace WebshopFrontend.Components.Layout;

public partial class LoginDisplay(IAuthAgent authAgent) : ComponentBase
{
    private async Task Logout()
    {
        await authAgent.LogoutAsync();
        
        NavManager.NavigateTo("/", forceLoad: true);
    }
}