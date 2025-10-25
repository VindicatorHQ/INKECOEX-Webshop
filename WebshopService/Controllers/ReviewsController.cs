using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebshopService.DTOs;
using WebshopService.DTOs.Requests;
using WebshopService.DTOs.Responses;
using WebshopService.Models;
using WebshopService.Repositories.Interface;

namespace WebshopService.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewController(IReviewRepository reviewRepository) : ControllerBase
{
    private string? GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);
    private bool IsAdmin() => User.IsInRole("Admin");

    [HttpGet("product/{productId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReviewResponse>))]
    public async Task<IActionResult> GetProductReviews(int productId)
    {
        var reviews = await reviewRepository.GetByProductIdAsync(productId);
        
        var response = reviews.Select(MapToReviewResponse);
        
        return Ok(response);
    }
    
    [HttpGet("guide/{guideId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReviewResponse>))]
    public async Task<IActionResult> GetGuideReviews(int guideId)
    {
        var reviews = await reviewRepository.GetByGuideIdAsync(guideId);
        
        var response = reviews.Select(MapToReviewResponse);
        
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReviewResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostReview([FromBody] ReviewRequest request)
    {
        var userId = GetCurrentUserId();
        
        if (request.ProductId.HasValue == request.GuideId.HasValue)
        {
            return BadRequest(new Error("Een review moet aan één ProductId OF één GuideId gekoppeld zijn, niet beide of geen.", "IWS400"));
        }

        var review = new Review
        {
            Comment = request.Comment,
            Rating = request.Rating,
            ProductId = request.ProductId,
            GuideId = request.GuideId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };
        
        if (userId != null)
        {
            review.ReviewerName = request.ReviewerName ?? User.Identity?.Name ?? "Geauthenticeerde Gebruiker"; 
        }
        else
        {
            review.ReviewerName = request.ReviewerName ?? "Anoniem";
        }
        
        await reviewRepository.AddAsync(review);
        
        var response = MapToReviewResponse(review);
        
        object routeValues = review.ProductId.HasValue 
            ? new { productId = review.ProductId } 
            : new { guideId = review.GuideId };
            
        return CreatedAtAction(nameof(GetProductReviews), routeValues, response);
    }
    
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReviewResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewRequest request)
    {
        var userId = GetCurrentUserId();
        var isAdmin = IsAdmin();
        
        if (userId == null) return Unauthorized(); 

        var review = await reviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            return NotFound(new Error($"Review met ID {id} niet gevonden.", "IWS404"));
        }

        if (review.UserId != userId && !isAdmin)
        {
            return Forbid("U heeft geen rechten om deze review aan te passen.", "IWS403");
        }

        review.Comment = request.Comment;
        review.Rating = request.Rating;
        
        await reviewRepository.UpdateAsync(review);
        
        return Ok(MapToReviewResponse(review));
    }
    
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var userId = GetCurrentUserId();
        var isAdmin = IsAdmin();
        
        if (userId == null) return Unauthorized();

        var review = await reviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            return NotFound(new Error($"Review met ID {id} niet gevonden.", "IWS404"));
        }

        if (review.UserId != userId && !isAdmin)
        {
            return Forbid("U heeft geen rechten om deze review te verwijderen.", "IWS403");
        }

        await reviewRepository.DeleteAsync(id);
        
        return NoContent();
    }
    
    private static ReviewResponse MapToReviewResponse(Review review)
    {
        return new ReviewResponse
        {
            Id = review.Id,
            ReviewerName = review.ReviewerName,
            Comment = review.Comment,
            Rating = review.Rating,
            CreatedAt = review.CreatedAt,
            UserId = review.UserId
        };
    }
}
