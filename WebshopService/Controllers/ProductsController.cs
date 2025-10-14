using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Responses;
using WebshopService.Repositories;

namespace WebshopService.Controllers;

[AllowAnonymous]
[Route("api/products")]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ProductResponse>))]
    public async Task<IActionResult> GetProducts()
    {
        var products = await productRepository.GetAllAsync();
        
        var response = products.Select(p => new ProductResponse 
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            CategoryNames = string.Join(", ", p.ProductCategories.Select(pc => pc.Category.Name))
        });
        
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(ProductResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        
        if (product == null)
        {
            return NotFound();
        }
        
        return Ok(product); 
    }
}