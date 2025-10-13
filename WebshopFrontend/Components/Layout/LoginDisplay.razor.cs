using Microsoft.AspNetCore.Components;
using WebshopFrontend.Providers;

namespace WebshopFrontend.Components.Layout;

public partial class LoginDisplay : ComponentBase
{
    private async Task Logout()
    {
        var jwtProvider = (JwtAuthenticationStateProvider)AuthProvider;
        await jwtProvider.MarkUserLoggedOut();
        
        NavManager.NavigateTo("/", forceLoad: true);
    }
}