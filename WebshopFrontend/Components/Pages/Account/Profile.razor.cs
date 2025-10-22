using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Account;

public partial class Profile(IUserAgent userAgent) : ComponentBase
{
    private UserProfileResponse? profile; 
    private bool updateSuccess;
    private string? errorMessage;

    protected override async Task OnInitializedAsync()
    {
        profile = await userAgent.GetUserProfileAsync();
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
                
                FullName = profile.FullName,
                Street = profile.Street,
                HouseNumber = profile.HouseNumber,
                ZipCode = profile.ZipCode,
                City = profile.City,
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
}