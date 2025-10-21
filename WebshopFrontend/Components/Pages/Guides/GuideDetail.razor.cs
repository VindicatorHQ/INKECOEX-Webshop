using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Guides;

public partial class GuideDetail(IGuideAgent guideAgent) : ComponentBase
{
    [Parameter]
    public string Slug { get; set; } = string.Empty;

    private GuideResponse? Guide;
    private bool isLoading = true;

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(Slug))
        {
            isLoading = true;
            Guide = await guideAgent.GetGuideBySlugAsync(Slug);
            isLoading = false;
            
            if (Guide == null)
            {
                NavigationManager.NavigateTo("/404", true);
            }
        }
    }
}