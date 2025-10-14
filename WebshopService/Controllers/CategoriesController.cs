using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Responses;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers;

[Route("api/categories")]
public class CategoriesController(ICategoryRepository categoryRepository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryResponse>))]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await categoryRepository.GetAllAsync();
        
        var response = categories.Select(c => new CategoryResponse { 
            Id = c.Id, 
            Name = c.Name 
        });
        
        return Ok(response);
    }
}