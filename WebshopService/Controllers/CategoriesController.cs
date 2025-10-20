using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Responses;
using WebshopService.Exceptions;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers;

[Route("api/categories")]
public class CategoriesController(ICategoryRepository categoryRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<CategoryResponse>>(StatusCodes.Status200OK, "application/json")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await categoryRepository.GetAllAsync();
        
        var response = categories.Select(c => new CategoryResponse { 
            Id = c.Id, 
            Name = c.Name,
            Slug = c.Slug
        });
        
        return Ok(response);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<CategoryResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<IActionResult> GetCategory(int id)
    {
        Category category;
        
        try
        {
            category = await categoryRepository.GetByIdAsync(id);
        }
        catch (CategoryNotFoundException exception)
        {
            return NotFound(new Error(exception.Message, "IWS404"));
        }
        
        return Ok(new CategoryResponse { Id = category.Id, Name = category.Name, Slug = category.Slug });
    }
}