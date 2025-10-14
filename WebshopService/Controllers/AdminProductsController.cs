using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.Constants;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
using WebshopService.Exceptions;
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
        
        return CreatedAtAction(nameof(CreateProduct), new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest request)
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
    [ProducesResponseType(404)]
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