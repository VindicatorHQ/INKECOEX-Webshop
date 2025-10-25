using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Products;

public partial class ProductDetail(IProductAgent productAgent, IShoppingCartAgent shoppingCartAgent) : ComponentBase
{
    [Parameter] public int ProductId { get; set; }

    private ProductResponse? product;
    private bool showSuccess;

    protected override async Task OnInitializedAsync()
    {
        product = await productAgent.GetProductByIdAsync(ProductId);
        
        if (product == null)
        {
            NavigationManager.NavigateTo("/products");
        }
    }
    
    private async Task AddToCart()
    {
        var updatedCart = await shoppingCartAgent.AddToCartAsync(ProductId, 1);

        if (updatedCart != null)
        {
            showSuccess = true;

            await Task.Delay(3000);
            
            showSuccess = false;
            
            await InvokeAsync(StateHasChanged);
        }
    }
}