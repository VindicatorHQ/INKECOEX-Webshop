using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Components.Pages.Auth;

public partial class Register(IAuthAgent authAgent) : ComponentBase
{
    private RegisterRequest registerRequest = new();
    private string? errorMessage;
    
    private async Task HandleRegistration()
    {
        errorMessage = null;

        var success = await authAgent.RegisterAsync(registerRequest);

        if (success)
        {
            NavigationManager.NavigateTo("/login", forceLoad: true);
        }
        else
        {
            errorMessage = "Registratie mislukt. Het e-mailadres is mogelijk al in gebruik, of probeer later opnieuw.";
        }
    }
}