using Microsoft.EntityFrameworkCore;
using WebshopService.Data;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Repositories.Implementation;

public class GuideRepository(WebshopDbContext context)  : IGuideRepository
{
    public async Task<IEnumerable<Guide>> GetAllAsync()
    {
        return await context.Guides
            .OrderByDescending(g => g.UpdatedAt)
            .ToListAsync();
    }

    public async Task<Guide?> GetByIdAsync(int id)
    {
        return await context.Guides.FindAsync(id);
    }

    public async Task<Guide?> GetBySlugAsync(string slug)
    {
        return await context.Guides.FirstOrDefaultAsync(g => g.Slug.Equals(slug));
    }

    public async Task<Guide> AddAsync(Guide entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        
        await context.Guides.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entity;
    }

    public async Task UpdateAsync(Guide entity)
    {
        var existing = await context.Guides.FindAsync(entity.Id);
        
        if (existing != null)
        {
            context.Entry(existing).CurrentValues.SetValues(entity);
            existing.UpdatedAt = DateTime.UtcNow;
            
            existing.Slug = entity.Slug; 
            
            context.Entry(existing).Property(g => g.CreatedAt).IsModified = false;
            
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var guide = await context.Guides.FindAsync(id);
        
        if (guide == null)
        {
            return false;
        }

        context.Guides.Remove(guide);
        await context.SaveChangesAsync();
        
        return true;
    }
}