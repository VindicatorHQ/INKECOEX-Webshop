using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Agents.Interface;

public interface IAuthAgent
{
    Task<bool> LoginAsync(LoginRequest model);
    Task LogoutAsync();
}