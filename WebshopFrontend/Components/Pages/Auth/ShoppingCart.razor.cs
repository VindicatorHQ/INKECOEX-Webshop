using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.Services;

namespace WebshopFrontend.Components.Pages.Auth;

public partial class ShoppingCart(IShoppingCartAgent shoppingCartAgent, ShoppingCartService cartService) : ComponentBase, IDisposable
{
    private bool isLoading = true;
    private string ErrorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        cartService.OnChange += HandleCartChanged;
        
        if (cartService.CurrentCart.Items.Count == 0)
        {
            await shoppingCartAgent.GetCartAsync();
        }

        isLoading = false;
    }

    private void HandleCartChanged()
    {
        InvokeAsync(StateHasChanged);
    }

    private async Task RemoveItem(int productId)
    {
        isLoading = true;
        ErrorMessage = string.Empty;
        
        var updatedCart = await shoppingCartAgent.RemoveFromCartAsync(productId);
        
        if (updatedCart == null)
        {
            ErrorMessage = "Er is iets fout gegaan tijdens het verwijderen van je item.";
            
            Console.WriteLine("Fout bij het verwijderen van item uit ShoppingCart.");
        }
        
        cartService.SetCart(updatedCart);
        isLoading = false;
    }
    
    private void StartCheckout()
    {
        NavigationManager.NavigateTo("/checkout");
    }

    public void Dispose()
    {
        cartService.OnChange -= HandleCartChanged;
    }
}