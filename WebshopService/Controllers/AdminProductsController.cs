using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.Constants;
using WebshopService.DTOs.Requests;
using WebshopService.Models;
using WebshopService.Repositories;

namespace WebshopService.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = Roles.Admin)]
public class AdminProductsController(IProductRepository productRepository) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request)
    {
        var newProduct = new Product 
        { 
            Name = request.Name, 
            Description = request.Description, 
            Price = request.Price, 
            StockQuantity = request.StockQuantity 
        };
        
        await productRepository.AddAsync(newProduct, request.CategoryIds);

        var allProducts = await productRepository.GetAllAsync();
        
        return CreatedAtAction(nameof(CreateProduct), new { id = newProduct.Id }, newProduct);
    }
}