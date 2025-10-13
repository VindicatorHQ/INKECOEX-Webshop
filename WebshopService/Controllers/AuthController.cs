using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebshopService.Data;
using WebshopService.DTOs;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;

namespace WebshopService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(UserManager<IdentityUser> userManager, IConfiguration config)
    : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(AuthResponse))]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized(new { message = "Ongeldige inloggegevens." });
        }

        var jwtHelper = new Jwt(userManager, config);

        var token = await jwtHelper.GenerateJwtToken(user);
        
        return Ok(token);
    }

    // TODO: Voeg een 'Register' endpoint toe voor nieuwe Consumers
    // [HttpPost("register")]
    // ...
}