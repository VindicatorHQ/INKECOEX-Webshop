using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.Constants;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
using WebshopService.Exceptions;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = Roles.Admin)]
public class AdminProductsController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(ProductResponse))]
    [ProducesResponseType(404, Type = typeof(Error))]
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
    
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ProductResponse))]
    [ProducesResponseType(400, Type = typeof(Error))]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
    
        var productEntity = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
        
            ProductCategories = request.CategoryIds
                .Select(categoryId => new ProductCategory() { CategoryId = categoryId })
                .ToList()
        };
    
        var createdProduct = await productRepository.CreateAsync(productEntity);

        var response = new ProductResponse
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Price = createdProduct.Price,
            StockQuantity = createdProduct.StockQuantity,
            CategoryNames = string.Join(", ", createdProduct.ProductCategories.Select(cp => cp.Category?.Name ?? "Onbekend"))
        };

        return CreatedAtAction(nameof(GetProduct), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400, Type = typeof(Error))]
    [ProducesResponseType(404, Type = typeof(Error))]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductCreateRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new Error("Product ID mismatch.", "IWS400"));
        }

        Product existingProduct;
        
        try
        {
            existingProduct = await productRepository.GetByIdAsync(id);
        }
        catch (ProductNotFoundException exception)
        {
            return NotFound(new Error(exception.Message, "IWS404"));
        }

        await productRepository.UpdateAsync(existingProduct, request);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = typeof(Error))]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            await productRepository.DeleteAsync(id);
            
            return NoContent();
        }
        catch (ProductNotFoundException exception)
        {
            return NotFound(new Error(exception.Message, "IWS404"));
        }
    }
}