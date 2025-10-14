using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages;

public partial class ProductCatalog(IProductAgent productAgent) : ComponentBase
{
    private List<ProductResponse> products = new();
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        products = await productAgent.GetProductsAsync();
        isLoading = false;
    }
}