using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Agents.Interface;

public interface IAnalyticsAgent
{
    Task<SalesDataResponse?> GetSalesStatisticsAsync(int days);
}