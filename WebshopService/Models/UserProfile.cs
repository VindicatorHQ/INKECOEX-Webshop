using Microsoft.AspNetCore.Identity;

namespace WebshopService.Models;

public class UserProfile
{
    public string UserId { get; set; } 
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public int? DefaultShippingAddressId { get; set; } 

    public ShippingAddress? DefaultShippingAddress { get; set; }
    
    public IdentityUser User { get; set; } = default!;
}