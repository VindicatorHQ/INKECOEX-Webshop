using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.Repositories;

namespace WebshopService.Controllers;

[Authorize]
[Route("api/[controller]")]
public class OrdersController(IOrderRepository orderRepository) : ControllerBase
{
    
}