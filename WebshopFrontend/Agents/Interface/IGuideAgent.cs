using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface IGuideAgent
{
    Task<List<GuideResponse>> GetAllGuidesAsync();
    Task<GuideResponse?> GetGuideBySlugAsync(string slug);
    Task<GuideResponse?> GetGuideByIdAsync(int id); 
    
    // Admin
    Task<GuideResponse?> CreateAsync(GuideRequest request);
    Task<bool> UpdateAsync(GuideRequest request);
    Task<bool> DeleteAsync(int id);
}