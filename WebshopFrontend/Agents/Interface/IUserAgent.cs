using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface IUserAgent
{
    Task<UserProfileResponse?> GetUserProfileAsync();
    Task<bool> UpdateUserProfileAsync(UserProfileRequest profile);
}