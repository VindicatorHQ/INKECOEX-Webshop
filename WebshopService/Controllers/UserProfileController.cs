using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class UserProfileController(IUserProfileRepository profileRepository) : ControllerBase
{
    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(UserProfileResponse))]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetUserId();
        
        var profile = await profileRepository.GetOrCreateByUserIdAsync(userId); 
        
        var response = new UserProfileResponse
        {
            Email = User.FindFirstValue(ClaimTypes.Email)!,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Street = profile.Street,
            City = profile.City,
            PostalCode = profile.PostalCode,
            Country = profile.Country
        };

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileRequest request)
    {
        var userId = GetUserId();
        
        var profile = await profileRepository.GetByUserIdAsync(userId); 
        
        if (profile == null)
        {
            return BadRequest(new { message = "Profiel kon niet worden gevonden of aangemaakt." });
        }

        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;
        profile.Street = request.Street;
        profile.City = request.City;
        profile.PostalCode = request.PostalCode;
        profile.Country = request.Country;

        await profileRepository.UpdateAsync(profile);

        return Ok();
    }
}