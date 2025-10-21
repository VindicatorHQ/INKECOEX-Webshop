using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Components.Pages.Admin;

public partial class GuideCreate(IGuideAgent guideAgent) : ComponentBase
{
    private GuideRequest GuideModel { get; set; } = new();
    private bool isSaving;
    private string errorMessage = string.Empty;

    private async Task HandleCreate()
    {
        isSaving = true;
        errorMessage = string.Empty;
        
        var newGuide = await guideAgent.CreateAsync(GuideModel);

        if (newGuide != null)
        {
            NavigationManager.NavigateTo("/admin/guides");
        }
        else
        {
            errorMessage = "Kon de gids niet aanmaken. Controleer of de titel al bestaat of probeer het later opnieuw.";
        }
        
        isSaving = false;
    }
}