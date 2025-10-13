using Microsoft.AspNetCore.Components;
using WebshopFrontend.DTOs;
using WebshopFrontend.Providers;

namespace WebshopFrontend.Components.Pages;

public partial class Login : ComponentBase
{
    private LoginRequest LoginRequest = new();
    private string ErrorMessage;

    private JwtAuthenticationStateProvider JwtProvider => (JwtAuthenticationStateProvider)AuthProvider; 

    private async Task HandleLogin()
    {
        ErrorMessage = string.Empty;
        
        try
        {
            var response = await Http.PostAsJsonAsync("api/auth/login", LoginRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                
                await JwtProvider.MarkUserAuthenticated(authResponse.Token);
                
                NavManager.NavigateTo("/", forceLoad: true); 
            }
            else
            {
                ErrorMessage = "Inloggen mislukt. Controleer uw e-mail en wachtwoord.";
            }
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Kan geen verbinding maken met de backend service.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Er is een onverwachte fout opgetreden: {ex.Message}";
        }
    }
}