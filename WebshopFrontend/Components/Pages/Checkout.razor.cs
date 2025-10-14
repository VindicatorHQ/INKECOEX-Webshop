using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;
using WebshopFrontend.Services;

namespace WebshopFrontend.Components.Pages;

public partial class Checkout(IOrderAgent orderAgent, ShoppingCartService cartService) : ComponentBase
{
    private CheckoutRequest checkoutRequest = new();
    private bool isProcessing;
    private string? errorMessage;

    private async Task HandleCheckout()
    {
        isProcessing = true;
        errorMessage = null;

        var orderId = await orderAgent.PlaceOrderAsync(checkoutRequest);

        if (orderId.HasValue)
        {
            cartService.SetCart(new ShoppingCartResponse());
            
            NavigationManager.NavigateTo($"/order/confirmation/{orderId.Value}");
        }
        else
        {
            // Toon foutmelding (bijv. onvoldoende voorraad, of algemene fout)
            errorMessage = "Kon de bestelling niet plaatsen. Controleer je adres of de voorraad.";
            
            isProcessing = false;
        }
    }
}