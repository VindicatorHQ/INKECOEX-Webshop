using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Components.Pages.Admin.Products;

public partial class ProductEdit(IProductAgent productAgent) : ComponentBase
{
    [Parameter] public int ProductId { get; set; }

    private ProductRequest? productRequest;
    private bool isLoading = true;
    private bool ShowError;

    protected override async Task OnInitializedAsync()
    {
        var productResponse = await productAgent.GetProductByIdAsync(ProductId);
        
        if (productResponse != null)
        {
            productRequest = new ProductRequest
            {
                Id = ProductId,
                Name = productResponse.Name,
                Description = productResponse.Description,
                Price = productResponse.Price,
                StockQuantity = productResponse.StockQuantity,
                
                CategoryIds = productResponse.CategoryIds
            };
        }
        isLoading = false;
    }

    private async Task HandleUpdate()
    {
        ShowError = false;
        
        if (productRequest == null) return;

        var success = await productAgent.UpdateAsync(ProductId, productRequest);

        if (success)
        {
            NavigationManager.NavigateTo("/admin/products");
        }
        else
        {
            ShowError = true;
        }
    }
}