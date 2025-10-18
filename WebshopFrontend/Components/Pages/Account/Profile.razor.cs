using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;
using WebshopFrontend.Services;

namespace WebshopFrontend.Components.Pages.Account;

public partial class Profile(IUserAgent userAgent, IThemeService themeService) : ComponentBase, IDisposable
{
    private UserProfileResponse? profile; 
    private bool updateSuccess;
    private string? errorMessage;

    protected override async Task OnInitializedAsync()
    {
        profile = await userAgent.GetUserProfileAsync();
        
        themeService.OnThemeChanged += StateHasChanged; 
    }

    private async Task HandleUpdate()
    {
        updateSuccess = false;
        errorMessage = null;

        if (profile != null)
        {
            var updateRequest = new UserProfileRequest
            {
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Street = profile.Street,
                City = profile.City,
                PostalCode = profile.PostalCode,
                Country = profile.Country
            };
            
            var success = await userAgent.UpdateUserProfileAsync(updateRequest);
            
            if (success)
            {
                updateSuccess = true;
            }
            else
            {
                errorMessage = "Opslaan mislukt. Probeer later opnieuw.";
            }
        }
    }
    
    private async Task ToggleTheme()
    {
        await themeService.ToggleThemeAsync();
    }
    
    public void Dispose()
    {
        themeService.OnThemeChanged -= StateHasChanged;
    }
}