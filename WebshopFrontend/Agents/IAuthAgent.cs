using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Agents;

public interface IAuthAgent
{
    Task<bool> LoginAsync(LoginRequest model);
    Task LogoutAsync();
}