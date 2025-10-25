using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents.Interface;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Pages.Admin.Orders;

public partial class OrderDetail(IOrderAgent orderAgent) : ComponentBase
{
    [Parameter] public int OrderId { get; set; }

    private OrderDetailResponse? order;
    private StatusUpdate statusUpdate = new();
    private bool isUpdating;
    private string? statusMessage;

    private class StatusUpdate {
        public string NewStatus { get; set; } = "New";
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadOrderDetails();
    }
    
    private async Task LoadOrderDetails()
    {
        order = await orderAgent.GetAdminOrderDetailAsync(OrderId);
        
        if (order != null)
        {
            statusUpdate.NewStatus = order.Status;
        }
        else
        {
            NavigateToAdminOrdersPage();
        }
    }

    private async Task HandleStatusUpdate()
    {
        isUpdating = true;
        statusMessage = null;

        var success = await orderAgent.UpdateOrderStatusAsync(OrderId, statusUpdate.NewStatus);

        if (success)
        {
            statusMessage = $"Status succesvol geüpdate naar: {statusUpdate.NewStatus}";

            await LoadOrderDetails(); 
        }
        else
        {
            statusMessage = "Fout bij het updaten van de status. Controleer je rechten.";
        }
        
        isUpdating = false;
    }

    private void NavigateToAdminOrdersPage()
    {
        NavigationManager.NavigateTo("/admin/orders");
    }
}