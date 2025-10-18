using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.DTOs.Responses;

public class UserProfileResponse : UserProfileRequest
{
    public string Email { get; set; } = string.Empty; 
}