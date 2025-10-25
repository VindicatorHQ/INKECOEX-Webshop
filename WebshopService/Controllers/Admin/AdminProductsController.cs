using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.Constants;
using WebshopService.DTOs;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
using WebshopService.Exceptions;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers.Admin;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = Roles.Admin)]
public class AdminProductsController(IProductRepository productRepository) : ProductsController(productRepository)
{
    [HttpPost]
    [ProducesResponseType<ProductResponse>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequest request)
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
                .Select(categoryId => new ProductCategory { CategoryId = categoryId })
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status400BadRequest, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest request)
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
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