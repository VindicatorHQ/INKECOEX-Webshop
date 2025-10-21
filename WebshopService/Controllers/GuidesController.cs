using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
using WebshopService.Models;
using WebshopService.Repositories.Interface;
using WebshopService.Utils; 

namespace WebshopService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GuidesController(IGuideRepository guideRepository) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous] 
    [ProducesResponseType<IEnumerable<GuideResponse>>(StatusCodes.Status200OK, "application/json")]
    public async Task<ActionResult<IEnumerable<GuideResponse>>> GetAll()
    {
        var guides = await guideRepository.GetAllAsync();
        
        var response = guides.Select(g => new GuideResponse
        {
            Id = g.Id,
            Title = g.Title,
            Slug = g.Slug,
            CreatedAt = g.CreatedAt
        });

        return Ok(response);
    }
    
    [HttpGet("{slug}")]
    [AllowAnonymous]
    [ProducesResponseType<GuideResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<ActionResult<GuideResponse>> GetBySlug(string slug)
    {
        var guide = await guideRepository.GetBySlugAsync(slug.ToLowerInvariant());

        if (guide == null)
        {
            return NotFound(new Error("De gevraagde gids is niet gevonden.", "IWS404"));
        }
        
        var response = new GuideResponse
        {
            Id = guide.Id,
            Title = guide.Title,
            Slug = guide.Slug,
            Content = guide.Content,
            CreatedAt = guide.CreatedAt,
            UpdatedAt = guide.UpdatedAt
        };

        return Ok(response);
    }
    
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType<GuideResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<ActionResult<GuideResponse>> GetById(int id)
    {
        var guide = await guideRepository.GetByIdAsync(id);

        if (guide == null)
        {
            return NotFound(new Error("De gevraagde gids is niet gevonden.", "IWS404"));
        }
        
        var response = new GuideResponse
        {
            Id = guide.Id,
            Title = guide.Title,
            Slug = guide.Slug,
            Content = guide.Content,
            CreatedAt = guide.CreatedAt,
            UpdatedAt = guide.UpdatedAt
        };

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType<GuideResponse>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status400BadRequest, "application/json")]
    public async Task<ActionResult<GuideResponse>> Create([FromBody] GuideRequest request)
    {
        var slug = request.Title.ToSlug(); 
        
        if (await guideRepository.GetBySlugAsync(slug) != null)
        {
            return BadRequest(new Error("Een gids met deze titel bestaat al. Gelieve een andere titel te kiezen.", "IWS400"));
        }
        
        var guide = new Guide
        {
            Title = request.Title,
            Content = request.Content,
            Slug = slug
        };

        var createdGuide = await guideRepository.AddAsync(guide);

        var response = new GuideResponse
        {
            Id = createdGuide.Id,
            Title = createdGuide.Title,
            Slug = createdGuide.Slug,
            Content = createdGuide.Content,
            CreatedAt = createdGuide.CreatedAt
        };
        
        return CreatedAtAction(nameof(GetBySlug), new { slug = response.Slug }, response);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status400BadRequest, "application/json")]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<ActionResult> Update(int id, [FromBody] GuideRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(new Error("ID in de URL komt niet overeen met ID in de request body.", "IWS400"));
        }

        var existingGuide = await guideRepository.GetByIdAsync(id);

        if (existingGuide == null)
        {
            return NotFound(new Error($"Gids met ID {id} niet gevonden.", "IWS404"));
        }

        existingGuide.Title = request.Title;
        existingGuide.Content = request.Content;
        existingGuide.UpdatedAt = DateTime.UtcNow;

        var newSlug = request.Title.ToSlug();
        if (existingGuide.Slug != newSlug)
        {
            var conflictGuide = await guideRepository.GetBySlugAsync(newSlug);
            if (conflictGuide != null && conflictGuide.Id != existingGuide.Id)
            {
                return BadRequest(new Error("Een andere gids met deze nieuwe titel/slug bestaat al.", "IWS400"));
            }
            existingGuide.Slug = newSlug;
        }

        await guideRepository.UpdateAsync(existingGuide);
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await guideRepository.DeleteAsync(id);
        
        if (!success)
        {
            return NotFound(new Error($"Gids met ID {id} niet gevonden om te verwijderen.", "IWS404"));
        }

        return NoContent();
    }
}
