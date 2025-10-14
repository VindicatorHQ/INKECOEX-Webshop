using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Admin;

public partial class ProductManagement(IProductAgent productAgent) : ComponentBase
{
    private List<ProductResponse> products = new();
    private string ErrorMessage { get; set; } = string.Empty;
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        products = await productAgent.GetProductsAsync(); 
        
        isLoading = false;
    }

    private async Task DeleteProduct(int id)
    {
        var success = await productAgent.DeleteProductAsync(id);
        
        if (success)
        {
            products.RemoveAll(p => p.Id == id);
            ErrorMessage = string.Empty;
        }
        else
        {
            ErrorMessage = "Something went wrong while deleting the product";
        }
    }
}