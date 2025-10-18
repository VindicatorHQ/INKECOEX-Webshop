using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Orders;

public partial class OrderDetail(IOrderAgent orderAgent) : ComponentBase
{
    [Parameter] public int OrderId { get; set; }

    private OrderDetailResponse? order;

    protected override async Task OnInitializedAsync()
    {
        order = await orderAgent.GetOrderDetailAsync(OrderId);

        if (order == null)
        {
            NavigateToMyOrdersPage();
        }
    }

    private void NavigateToMyOrdersPage()
    {
        NavigationManager.NavigateTo("/orders");
    }
}