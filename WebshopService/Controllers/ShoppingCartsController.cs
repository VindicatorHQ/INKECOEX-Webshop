using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers;

[ApiController]
[Route("api/carts")]
[Authorize]
public class ShoppingCartsController(IShoppingCartRepository shoppingCartRepository) : ControllerBase
{
    private string GetUserId() 
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(ShoppingCartResponse))]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetUserId();
        var cart = await shoppingCartRepository.GetByUserIdAsync(userId);
        
        if (cart == null)
        {
            return Ok(new ShoppingCartResponse());
        }

        var response = new ShoppingCartResponse
        {
            Items = cart.CartItems.Select(item => new CartItemResponse
            {
                CartItemId = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name ?? "Onbekend",
                Price = item.Product?.Price ?? 0,
                Quantity = item.Quantity
            }).ToList()
        };
        
        return Ok(response);
    }

    [HttpPost("items")]
    [ProducesResponseType(200, Type = typeof(ShoppingCartResponse))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var userId = GetUserId();
        var cart = await shoppingCartRepository.GetByUserIdAsync(userId) 
                   ?? new ShoppingCart { UserId = userId };

        var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == request.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
        }
        else
        {
            cart.CartItems.Add(new CartItem 
            { 
                ProductId = request.ProductId, 
                Quantity = request.Quantity 
            });
        }

        await shoppingCartRepository.SaveAsync(cart);
        
        return await GetCart();
    }
    
    [HttpDelete("items/{productId:int}")]
    [ProducesResponseType(200, Type = typeof(ShoppingCartResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userId = GetUserId();
        var cart = await shoppingCartRepository.GetByUserIdAsync(userId);

        if (cart == null)
        {
            return NotFound("Winkelwagen niet gevonden.");
        }

        var itemToRemove = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

        if (itemToRemove == null)
        {
            return NotFound("Product niet gevonden in winkelwagen.");
        }
        
        cart.CartItems.Remove(itemToRemove);
        await shoppingCartRepository.SaveAsync(cart);

        return await GetCart();
    }
}