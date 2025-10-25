using WebshopService.DTOs.Responses;

namespace WebshopService.Repositories.Interface;

public interface IAnalyticsRepository
{
    Task<SalesDataResponse> GetSalesStatisticsAsync(int days);
}