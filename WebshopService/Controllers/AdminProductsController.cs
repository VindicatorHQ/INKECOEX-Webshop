using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.Constants;
using WebshopService.Models;
using WebshopService.Repositories;

namespace WebshopService.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = Roles.Admin)]
public class AdminProductsController(IProductRepository productRepository) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateProduct(Product newProduct)
    {
        return Ok($"Product {newProduct.Name} aangemaakt door Admin.");
    }
}