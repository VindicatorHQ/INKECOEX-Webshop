using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Guides;

public partial class GuideOverview(IGuideAgent guideAgent) : ComponentBase
{
    private List<GuideResponse> guides = [];
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        guides = await guideAgent.GetAllGuidesAsync();
        
        isLoading = false;
    }
}