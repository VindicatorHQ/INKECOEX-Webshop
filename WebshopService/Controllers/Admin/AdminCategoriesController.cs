using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebshopService.DTOs;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
using WebshopService.Exceptions;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers.Admin;

[ApiController]
[Route("api/admin/categories")]
[Authorize(Roles = "Admin")]
public class AdminCategoriesController(ICategoryRepository categoryRepository) : CategoriesController(categoryRepository)
{
    [HttpPost]
    [ProducesResponseType<CategoryResponse>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new Error("Naam is vereist.", "IWS400"));
        }
        
        var category = new Category
        {
            Name = request.Name,
            Slug = request.Slug
        };
        
        await categoryRepository.AddAsync(category);

        var response = new CategoryResponse
        {
            Id = category.Id, 
            Name = category.Name,
            Slug = category.Slug
        };
        
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status400BadRequest, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new Error("Naam is vereist.", "IWS400"));
        }
        
        Category existingCategory;
        
        try
        {
            existingCategory = await categoryRepository.GetByIdAsync(id);
        }
        catch (CategoryNotFoundException exception)
        {
            return NotFound(new Error(exception.Message, "IWS404"));
        }

        await categoryRepository.UpdateAsync(existingCategory, request);
        
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status409Conflict, "application/json")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            await categoryRepository.DeleteAsync(id);
            
            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict(new Error("Kan categorie niet verwijderen omdat er nog producten aan gekoppeld zijn.", "IWS409"));
        }
        catch (Exception exception)
        {
            return NotFound(new Error(exception.Message, "IWS404"));
        }
    }
}