using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Components.Pages.Admin.Guides;

public partial class GuideEdit(IGuideAgent guideAgent) : ComponentBase
{
    [Parameter]
    public int Id { get; set; }

    private GuideRequest GuideModel { get; set; } = new();
    private bool isLoading = true;
    private bool isSaving;
    private string errorMessage = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        if (Id > 0)
        {
            isLoading = true;
            
            var guideToEdit = await guideAgent.GetGuideByIdAsync(Id);

            if (guideToEdit != null)
            {
                GuideModel = new GuideRequest
                {
                    Id = guideToEdit.Id,
                    Title = guideToEdit.Title,
                    Content = guideToEdit.Content
                };
            }
            else
            {
                GuideModel.Id = null;
            }

            isLoading = false;
        }
    }

    private async Task HandleUpdate()
    {
        isSaving = true;
        errorMessage = string.Empty;

        if (!GuideModel.Id.HasValue)
        {
            errorMessage = "Fout: De gids ID is niet ingesteld.";
            isSaving = false;
            return;
        }
        
        var success = await guideAgent.UpdateAsync(GuideModel);

        if (success)
        {
            NavigationManager.NavigateTo("/admin/guides");
        }
        else
        {
            errorMessage = "Kon de gids niet opslaan. Controleer de invoer of probeer het later opnieuw.";
        }
        
        isSaving = false;
    }
}