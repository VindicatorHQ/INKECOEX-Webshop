using Microsoft.AspNetCore.Components;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Admin;

public partial class ProductForm : ComponentBase
{
    [Parameter] public ProductCreateRequest ProductRequest { get; set; } = new();
    [Parameter] public EventCallback OnValidSubmit { get; set; }
    [Parameter] public string Title { get; set; } = "Nieuw Product";
    [Parameter] public string ButtonText { get; set; } = "Opslaan";

    private List<CategoryResponse>? categories;

    protected override async Task OnInitializedAsync()
    {
        categories = await CategoryAgent.GetCategoriesAsync();
    }
    
    private void ToggleCategory(int categoryId)
    {
        if (!ProductRequest.CategoryIds.Remove(categoryId))
        {
            ProductRequest.CategoryIds.Add(categoryId);
        }
    }
}