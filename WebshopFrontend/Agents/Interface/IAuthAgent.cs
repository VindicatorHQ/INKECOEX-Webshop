using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Agents.Interface;

public interface IAuthAgent
{
    Task<bool> LoginAsync(LoginRequest model);
    Task<bool> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
}