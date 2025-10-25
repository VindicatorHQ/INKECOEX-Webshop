using Microsoft.EntityFrameworkCore;
using WebshopService.Data;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Repositories.Implementation;

public class ReviewRepository(WebshopDbContext context) : IReviewRepository
{
    public async Task<List<Review>> GetByProductIdAsync(int productId)
    {
        return await context.Reviews
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Review>> GetByGuideIdAsync(int guideId)
    {
        return await context.Reviews
            .Where(r => r.GuideId == guideId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetByIdAsync(int reviewId)
    {
        return await context.Reviews
            .Include(r => r.Product) 
            .Include(r => r.Guide)
            .FirstOrDefaultAsync(r => r.Id == reviewId);
    }

    public async Task AddAsync(Review review)
    {
        context.Reviews.Add(review);
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Review review)
    {
        context.Reviews.Update(review);
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int reviewId)
    {
        var review = await GetByIdAsync(reviewId);
        
        if (review != null)
        {
            context.Reviews.Remove(review);
            
            await context.SaveChangesAsync();
        }
    }
}
