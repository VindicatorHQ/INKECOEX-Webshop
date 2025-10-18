using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebshopService.Data;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Repositories.Implementation;

public class UserProfileRepository(WebshopDbContext context, UserManager<IdentityUser> userManager)
    : IUserProfileRepository
{
    public async Task<UserProfile?> GetByUserIdAsync(string userId)
    {
        return await context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }
    
    public async Task<UserProfile> GetOrCreateByUserIdAsync(string userId)
    {
        var profile = await GetByUserIdAsync(userId);

        if (profile != null)
        {
            return profile;
        }
        
        profile = new UserProfile { UserId = userId };
        
        context.UserProfiles.Add(profile);
        await context.SaveChangesAsync();

        return profile;
    }

    public async Task UpdateAsync(UserProfile profile)
    {
        context.UserProfiles.Update(profile);
        
        await context.SaveChangesAsync();
    }
}