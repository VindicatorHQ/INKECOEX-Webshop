using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Admin.Analytics;

public partial class AdminDashboard(IAnalyticsAgent analyticsAgent) : ComponentBase
{
    private SalesDataResponse? SalesData;
    private bool IsLoading = true;
    private int SelectedDays = 30;

    protected override async Task OnInitializedAsync()
    {
        await LoadStatisticsAsync();
    }

    private async Task ChangePeriod(int days)
    {
        SelectedDays = days;
        await LoadStatisticsAsync();
    }

    private async Task LoadStatisticsAsync()
    {
        IsLoading = true;
        SalesData = null;
        try
        {
            SalesData = await analyticsAgent.GetSalesStatisticsAsync(SelectedDays);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij het laden van statistieken: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }
}