using WebshopFrontend.Providers;

namespace WebshopFrontend.Components.Layout;

public partial class MainLayout
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var jwtProvider = (JwtAuthenticationStateProvider)AuthProvider;
            
            await jwtProvider.InitializeAsync();
        }
    }
}