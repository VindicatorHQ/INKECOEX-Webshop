using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers.Admin;

[ApiController]
[Route("api/admin/orders")]
[Authorize(Roles = "Admin")]
public class AdminOrdersController(IOrderRepository orderRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<OrderSummaryResponse>>(StatusCodes.Status200OK, "application/json")]
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
    [ProducesResponseType<OrderDetailResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [HttpPut("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status400BadRequest, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewStatus))
        {
            return BadRequest(new Error("Status mag niet leeg zijn.", "IWS400"));
        }
        
        try
        {
            await orderRepository.UpdateOrderStatusAsync(id, request.NewStatus);
            
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new Error(exception.Message, "IWS404"));
        }
        catch (Exception)
        {
            return StatusCode(500, "Fout bij het updaten van de status.");
        }
    }
}