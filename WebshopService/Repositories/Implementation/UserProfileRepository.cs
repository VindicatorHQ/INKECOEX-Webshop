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
            .Include(p => p.DefaultShippingAddress)
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

    public async Task UpdateProfileAndDefaultAddressAsync(
        UserProfile profile, 
        string? fullName, 
        string? street, 
        string? houseNumber, 
        string? zipCode, 
        string? city, 
        string? country)
    {
        context.UserProfiles.Update(profile);

        ShippingAddress? defaultAddress;

        if (profile.DefaultShippingAddressId.HasValue)
        {
            defaultAddress = profile.DefaultShippingAddress ?? 
                             await context.ShippingAddresses.FindAsync(profile.DefaultShippingAddressId.Value);
        }
        else
        {
            defaultAddress = new ShippingAddress();
            context.ShippingAddresses.Add(defaultAddress);
        }

        if (defaultAddress != null)
        {
            defaultAddress.FullName = fullName ?? string.Empty;
            defaultAddress.Street = street ?? string.Empty;
            defaultAddress.HouseNumber = houseNumber ?? string.Empty;
            defaultAddress.ZipCode = zipCode ?? string.Empty;
            defaultAddress.City = city ?? string.Empty;
            defaultAddress.Country = country ?? string.Empty;

            if (!profile.DefaultShippingAddressId.HasValue)
            {
                await context.SaveChangesAsync(); 
                
                profile.DefaultShippingAddressId = defaultAddress.Id;
                profile.DefaultShippingAddress = defaultAddress;
                context.UserProfiles.Update(profile);
            }
        }
        
        await context.SaveChangesAsync();
    }
}
