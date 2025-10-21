using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Admin;

public partial class GuideManagement(IGuideAgent guideAgent) : ComponentBase
{
    private List<GuideResponse> guides = [];
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await LoadGuides();
        
        isLoading = false;
    }

    private async Task LoadGuides()
    {
        guides = await guideAgent.GetAllGuidesAsync();
    }

    private void NavigateToCreate()
    {
        NavigationManager.NavigateTo("/admin/guides/create");
    }

    private void NavigateToEdit(int id)
    {
        NavigationManager.NavigateTo($"/admin/guides/edit/{id}");
    }

    private async Task DeleteGuide(int id)
    {        
        object[] arguments = ["Weet je zeker dat je deze categorie wilt verwijderen?"];
        
        if (await JsRuntime.InvokeAsync<bool>("confirm", arguments))
        {
            var success = await guideAgent.DeleteAsync(id);

            if (success)
            {
                guides.RemoveAll(g => g.Id == id);
            
                StateHasChanged();
            }
            else
            {
                Console.WriteLine("Fout bij verwijderen.");
            }
        }
    }
}