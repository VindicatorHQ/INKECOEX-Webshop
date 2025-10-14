using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Requests;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers;

[Authorize]
[Route("api/[controller]")]
public class OrdersController(IOrderRepository orderRepository) : ControllerBase
{
    private string GetUserId() 
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(int))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PlaceOrder([FromBody] CheckoutRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var address = new ShippingAddress
            {
                FullName = request.FullName,
                Street = request.Street,
                HouseNumber = request.HouseNumber,
                ZipCode = request.ZipCode,
                City = request.City,
                Country = request.Country
            };
            
            var userId = GetUserId();
            var orderId = await orderRepository.PlaceOrderAsync(userId, address);

            return StatusCode(201, orderId); 
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Er is een interne fout opgetreden bij het plaatsen van de bestelling.");
        }
    }
}