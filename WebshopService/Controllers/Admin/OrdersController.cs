using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Responses;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers.Admin;

[ApiController]
[Route("api/admin/orders")]
[Authorize(Roles = "Admin")]
public class OrdersController(IOrderRepository orderRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OrderSummaryResponse>))]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await orderRepository.GetAllOrdersAsync();

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
    [ProducesResponseType(200, Type = typeof(OrderDetailResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOrderDetails(int id)
    {
        var order = await orderRepository.GetOrderDetailsByIdAsync(id);

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

    public record UpdateStatusRequest(string NewStatus);

    [HttpPut("{id:int}/status")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewStatus))
        {
            return BadRequest("Status mag niet leeg zijn.");
        }
        
        try
        {
            await orderRepository.UpdateOrderStatusAsync(id, request.NewStatus);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "Fout bij het updaten van de status.");
        }
    }
}