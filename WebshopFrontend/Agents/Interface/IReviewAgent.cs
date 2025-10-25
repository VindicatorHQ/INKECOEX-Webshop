using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface IReviewAgent
{
    Task<List<ReviewResponse>> GetProductReviewsAsync(int productId);
    Task<List<ReviewResponse>> GetGuideReviewsAsync(int guideId);

    Task<ReviewResponse?> CreateReviewAsync(ReviewRequest request);
    Task<ReviewResponse?> UpdateReviewAsync(int reviewId, ReviewRequest request);
    Task<bool> DeleteReviewAsync(int reviewId);
}