using Blazored.SessionStorage;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;
using WebshopFrontend.Services;

namespace WebshopFrontend.Agents.Implementation;

public class ShoppingCartAgent(AgentUrl<ShoppingCartAgent> agentUrl, ShoppingCartService cartService, ISessionStorageService sessionStorage, NavigationManager NavigationManager) : IShoppingCartAgent
{
    private readonly string _baseUrl = agentUrl.Url;

    private async Task<IFlurlRequest> GetAuthorizedRequest(string path)
    {
        var token = await sessionStorage.GetItemAsStringAsync("authToken");

        return _baseUrl
            .AppendPathSegment(path)
            .WithOAuthBearerToken(token);
    }

    public async Task<ShoppingCartResponse?> GetCartAsync()
    {
        try
        {
            var authRequest = await GetAuthorizedRequest("api/carts");
            
            var cart = await authRequest.GetJsonAsync<ShoppingCartResponse>();
            
            if (cart != null) 
            {
                cartService.SetCart(cart);
            }
            
            return cart;
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 401)
        {
            Console.WriteLine($"Status 401: {ex.Message}");
            
            NavigationManager.NavigateTo("/login", forceLoad: true);
            
            return null; 
        }
    }
    
    public async Task<ShoppingCartResponse?> AddToCartAsync(int productId, int quantity)
    {
        try
        {
            var request = new { ProductId = productId, Quantity = quantity };
            
            var authRequest = await GetAuthorizedRequest("api/carts/items");
            
            var updatedCart = await authRequest
                .PostJsonAsync(request)
                .ReceiveJson<ShoppingCartResponse>();
            
            if (updatedCart != null)
            {
                cartService.SetCart(updatedCart);
            }
            
            return updatedCart;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Status {ex.StatusCode}: {ex.Message}");
            
            return null;
        }
    }
    
    public async Task<ShoppingCartResponse?> RemoveFromCartAsync(int productId)
    {
        try
        {
            var authRequest = await GetAuthorizedRequest($"api/carts/items/{productId}"); 
        
            return await authRequest
                .DeleteAsync()
                .ReceiveJson<ShoppingCartResponse>();
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Status {ex.StatusCode}: {ex.Message}");
            
            return null;
        }
    }
}