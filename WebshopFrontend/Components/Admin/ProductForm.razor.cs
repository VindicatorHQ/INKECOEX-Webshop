using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Admin;

public partial class ProductForm(ICategoryAgent categoryAgent) : ComponentBase
{
    [Parameter] public ProductRequest ProductRequest { get; set; } = new();
    [Parameter] public EventCallback OnValidSubmit { get; set; }
    [Parameter] public string Title { get; set; } = "Nieuw Product";
    [Parameter] public string ButtonText { get; set; } = "Opslaan";

    private List<CategoryResponse>? categories;

    protected override async Task OnInitializedAsync()
    {
        categories = await categoryAgent.GetAllCategoriesAsync();
    }
    
    private void ToggleCategory(int categoryId)
    {
        if (!ProductRequest.CategoryIds.Contains(categoryId))
        {
            ProductRequest.CategoryIds.Add(categoryId);
        }
        
        StateHasChanged();
    }
}