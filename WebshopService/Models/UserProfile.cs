using Microsoft.AspNetCore.Identity;

namespace WebshopService.Models;

public class UserProfile
{
    public string UserId { get; set; } 
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    
    public IdentityUser User { get; set; } = default!;
}