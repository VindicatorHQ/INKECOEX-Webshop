using WebshopService.DTOs.Requests;

namespace WebshopService.DTOs.Responses;

public class UserProfileResponse : UserProfileRequest
{
    public string Email { get; set; } = string.Empty; 
}