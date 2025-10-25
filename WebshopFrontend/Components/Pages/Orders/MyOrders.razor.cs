using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Orders;

public partial class MyOrders(IOrderAgent orderAgent) : ComponentBase
{
    private List<OrderSummaryResponse>? orders;

    protected override async Task OnInitializedAsync()
    {
        var result = await orderAgent.GetMyOrdersAsync();
        orders = result ?? [];
    }
}