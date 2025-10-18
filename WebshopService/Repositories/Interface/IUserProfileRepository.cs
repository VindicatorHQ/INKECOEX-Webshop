using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(string userId);
    Task<UserProfile> GetOrCreateByUserIdAsync(string userId);
    Task UpdateAsync(UserProfile profile);
}