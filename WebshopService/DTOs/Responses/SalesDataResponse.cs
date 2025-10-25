namespace WebshopService.DTOs.Responses;

public class SalesDataResponse
{
    public decimal TotalRevenue { get; set; }

    public int TotalOrders { get; set; }

    public List<DataPoint> RevenueByPeriod { get; set; } = [];

    public List<DataPoint> OrdersByPeriod { get; set; } = [];
}