using WebshopService.DTOs.Requests;
using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
    
    // Admin
    Task AddAsync(Category category);
    Task UpdateAsync(Category category, CategoryRequest request);
    Task DeleteAsync(int id);
}