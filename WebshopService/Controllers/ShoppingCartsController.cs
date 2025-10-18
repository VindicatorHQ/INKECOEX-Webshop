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
    [ProducesResponseType<ShoppingCartResponse>(StatusCodes.Status200OK, "application/json")]
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
    [ProducesResponseType<ShoppingCartResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var userId = GetUserId();
        var cart = await shoppingCartRepository.GetByUserIdAsync(userId) ?? new ShoppingCart { UserId = userId };

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
    [ProducesResponseType<ShoppingCartResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userId = GetUserId();
        var cart = await shoppingCartRepository.GetByUserIdAsync(userId);

        if (cart == null)
        {
            return NotFound(new Error("Winkelwagen niet gevonden.", "IWS404"));
        }

        var itemToModify = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

        if (itemToModify == null)
        {
            return NotFound(new Error("Product niet gevonden in winkelwagen.", "IWS404"));
        }
    
        if (itemToModify.Quantity > 1)
        {
            itemToModify.Quantity -= 1;
        }
        else
        {
            cart.CartItems.Remove(itemToModify);
        }
    
        await shoppingCartRepository.SaveAsync(cart);

        return await GetCart();
    }
}