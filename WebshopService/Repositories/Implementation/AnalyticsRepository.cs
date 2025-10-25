using WebshopService.Constants;
using WebshopService.DTOs;
using WebshopService.DTOs.Responses;
using WebshopService.Repositories.Interface;

namespace WebshopService.Repositories.Implementation;

public class AnalyticsRepository(IOrderRepository orderRepository) : IAnalyticsRepository
{
    public async Task<SalesDataResponse> GetSalesStatisticsAsync(int days)
    {
        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-days);

        var allOrders = await orderRepository.GetAllOrdersAsync();

        var ordersInPeriod = allOrders
            .Where(o => o.Status is OrderStatus.Shipped or OrderStatus.Delivered) 
            .Where(o => o.OrderDate >= startDate)
            .ToList();

        var totalRevenue = ordersInPeriod.Sum(o => o.Items.Sum(oi => oi.PriceAtOrder * oi.Quantity));
        var totalOrders = ordersInPeriod.Count;

        var dailyData = ordersInPeriod
            .GroupBy(o => o.OrderDate.Date)
            .Select(g => new
            {
                Date = g.Key,
                Revenue = g.Sum(o => o.Items.Sum(oi => oi.PriceAtOrder * oi.Quantity)),
                Orders = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToList();

        var revenueByPeriod = new List<DataPoint>();
        var ordersByPeriod = new List<DataPoint>();

        for (int i = 0; i < days; i++)
        {
            var date = startDate.AddDays(i);
            var dataForDay = dailyData.FirstOrDefault(d => d.Date == date);

            var label = date.ToString("dd-MM-yyyy");

            revenueByPeriod.Add(new DataPoint
            {
                Label = label,
                Value = (double)(dataForDay?.Revenue ?? 0m)
            });

            ordersByPeriod.Add(new DataPoint
            {
                Label = label,
                Value = dataForDay?.Orders ?? 0
            });
        }

        return new SalesDataResponse
        {
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            RevenueByPeriod = revenueByPeriod,
            OrdersByPeriod = ordersByPeriod
        };
    }
}
