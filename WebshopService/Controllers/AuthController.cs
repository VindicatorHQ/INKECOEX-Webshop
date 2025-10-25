using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebshopService.Constants;
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

    [HttpPost("register")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var userExists = await userManager.FindByEmailAsync(model.Email);
        
        if (userExists != null)
        {
            return BadRequest(new Error("E-mailadres is al in gebruik.", "IWS400"));
        }

        var user = new IdentityUser
        {
            Email = model.Email,
            UserName = model.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            
            return BadRequest(new { message = "Gebruiker aanmaken mislukt.", errors });
        }

        await userManager.AddToRoleAsync(user, Roles.Consumer);

        return StatusCode(201, new { message = "Gebruiker succesvol geregistreerd." });
    }
}