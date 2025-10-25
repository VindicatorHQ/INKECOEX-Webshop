using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface IReviewRepository
{
    Task<List<Review>> GetByProductIdAsync(int productId);
    Task<List<Review>> GetByGuideIdAsync(int guideId);
    Task<Review?> GetByIdAsync(int reviewId);
    Task AddAsync(Review review);

    Task UpdateAsync(Review review);
    Task DeleteAsync(int reviewId);
}