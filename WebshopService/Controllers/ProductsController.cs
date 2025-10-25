using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs;
using WebshopService.DTOs.Responses;
using WebshopService.Exceptions;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers;

[Route("api/products")]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<ProductResponse>>(StatusCodes.Status200OK, "application/json")]
    public async Task<IActionResult> GetProducts([FromQuery] string? category, [FromQuery] string? search) 
    {
        var products = await productRepository.GetAllAsync(
            searchTerm: search, 
            categorySlug: category
        );
        
        var response = products.Select(product => new ProductResponse 
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryIds = product.ProductCategories.Select(pc => pc.Category.Id).ToList(),
            CategoryNames = string.Join(", ", product.ProductCategories.Select(cp => cp.Category.Name))
        });
        
        return Ok(response);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> GetProduct(int id)
    {
        Product product;
        
        try
        {
            product = await productRepository.GetByIdAsync(id);
        }
        catch (ProductNotFoundException exception)
        {
            return NotFound(new Error(exception.Message, "IWS404"));
        }
        
        var response = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryIds = product.ProductCategories.Select(pc => pc.Category.Id).ToList(),
            CategoryNames = string.Join(", ", product.ProductCategories.Select(cp => cp.Category.Name))
        };
        
        return Ok(response);
    }
}