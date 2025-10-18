using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
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
    [ProducesResponseType<int>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status400BadRequest, "application/json")]
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
            return BadRequest(new Error(ex.Message, "IWS400"));
        }
        catch (Exception)
        {
            return StatusCode(500, "Er is een interne fout opgetreden bij het plaatsen van de bestelling.");
        }
    }
    
    [HttpGet]
    [ProducesResponseType<IEnumerable<OrderSummaryResponse>>(StatusCodes.Status200OK, "application/json")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = GetUserId();
        var orders = await orderRepository.GetOrdersByUserIdAsync(userId);

        var response = orders.Select(o => new OrderSummaryResponse
        {
            OrderId = o.Id,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Status = o.Status
        });

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<OrderDetailResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderDetails(int id)
    {
        var userId = GetUserId();
        var order = await orderRepository.GetOrderDetailAsync(id, userId);

        if (order == null)
        {
            return NotFound();
        }
    
        var response = new OrderDetailResponse
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
        
            Items = order.Items.Select(item => new OrderItemResponse
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                PriceAtOrder = item.PriceAtOrder,
                Quantity = item.Quantity
            }).ToList(),
        
            FullName = order.ShippingAddress?.FullName ?? string.Empty,
            AddressLine1 = $"{order.ShippingAddress?.Street} {order.ShippingAddress?.HouseNumber}",
            AddressLine2 = $"{order.ShippingAddress?.ZipCode} {order.ShippingAddress?.City}",
            Country = order.ShippingAddress?.Country ?? string.Empty
        };

        return Ok(response);
    }
}