using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Admin.Orders;

public partial class OrderManagement(IOrderAgent orderAgent) : ComponentBase
{
    private List<OrderSummaryResponse>? orders;

    protected override async Task OnInitializedAsync()
    {
        var result = await orderAgent.GetAllOrdersForAdminAsync();
        orders = result ?? [];
    }
}