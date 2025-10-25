using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface IGuideRepository
{
    Task<IEnumerable<Guide>> GetAllAsync();
    Task<Guide?> GetByIdAsync(int id);
    Task<Guide?> GetBySlugAsync(string slug);
    
    // Admin
    Task<Guide> AddAsync(Guide entity);
    Task UpdateAsync(Guide entity);
    Task<bool> DeleteAsync(int id);
}