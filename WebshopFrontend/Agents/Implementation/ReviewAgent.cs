using Flurl;
using Flurl.Http;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;
using Blazored.SessionStorage;

namespace WebshopFrontend.Agents.Implementation;

public class ReviewAgent(AgentUrl<ReviewAgent> agentUrl, ISessionStorageService sessionStorage) : IReviewAgent
{
    private readonly string _baseUrl = agentUrl.Url;
    private readonly SessionStorage _sessionStorage = new(sessionStorage);
    
    public async Task<List<ReviewResponse>> GetProductReviewsAsync(int productId)
    {
        return await GetReviewsAsync(_baseUrl.AppendPathSegment($"api/reviews/product/{productId}"));
    }

    public async Task<List<ReviewResponse>> GetGuideReviewsAsync(int guideId)
    {
        return await GetReviewsAsync(_baseUrl.AppendPathSegment($"api/reviews/guide/{guideId}"));
    }

    private async Task<List<ReviewResponse>> GetReviewsAsync(Url baseUrl)
    {
        try
        {
            var reviews = await baseUrl.GetJsonAsync<List<ReviewResponse>>();
            
            return reviews ?? [];
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error fetching reviews from {baseUrl}: {ex.Message}");
            
            return [];
        }
    }

    public async Task<ReviewResponse?> CreateReviewAsync(ReviewRequest request)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl, "api/reviews");
            
            var response = await authRequest
                .PostJsonAsync(request)
                .ReceiveJson<ReviewResponse>();
            
            return response;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error creating review: {ex.Message}");
            
            return null;
        }
    }

    public async Task<ReviewResponse?> UpdateReviewAsync(int reviewId, ReviewRequest request)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl, $"api/reviews/{reviewId}");
            
            var updatedReview = await authRequest
                .PutJsonAsync(request)
                .ReceiveJson<ReviewResponse>();
            
            return updatedReview;
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 403)
        {
            Console.WriteLine("Autorisatiefout: U mag deze review niet bewerken (403 Forbidden).");
            
            return null;
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 404)
        {
            Console.WriteLine($"Review niet gevonden (404 Not Found): {ex.Message}");
            
            return null;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error updating review: {ex.Message}");
            
            return null;
        }
    }

    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        try
        {
            var authRequest = await _sessionStorage.GetAuthorizedRequest(_baseUrl, $"api/reviews/{reviewId}");

            await authRequest.DeleteAsync();

            return true;
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 403)
        {
            Console.WriteLine("Autorisatiefout: U mag deze review niet verwijderen (403 Forbidden).");
            
            return false;
        }
        catch (FlurlHttpException ex) when (ex.Call.Response.StatusCode == 404)
        {
            Console.WriteLine($"Review niet gevonden (404 Not Found): {ex.Message}");
            
            return false;
        }
        catch (FlurlHttpException ex)
        {
            Console.WriteLine($"Flurl Error deleting review: {ex.Message}");
            
            return false;
        }
    }
}
