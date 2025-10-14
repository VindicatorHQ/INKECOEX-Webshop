using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
}