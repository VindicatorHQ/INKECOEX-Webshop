using WebshopService.Models;

namespace WebshopService.Repositories.Interface;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(string userId);
    Task<UserProfile> GetOrCreateByUserIdAsync(string userId);
    Task UpdateProfileAndDefaultAddressAsync(
        UserProfile profile, 
        string? fullName, 
        string? street, 
        string? houseNumber, 
        string? zipCode, 
        string? city, 
        string? country
    );
}