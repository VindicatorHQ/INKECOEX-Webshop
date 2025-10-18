using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Products;

public partial class ProductCatalog(IProductAgent productAgent) : ComponentBase
{
    private List<ProductResponse> products = new();
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        products = await productAgent.GetAsync();
        isLoading = false;
    }

    private void NavigateToProductPage(int id)
    {
        NavigationManager.NavigateTo($"/products/{id}");
    }
}