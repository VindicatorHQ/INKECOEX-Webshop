using Microsoft.AspNetCore.Components;

namespace WebshopFrontend.Components.Modals;

public partial class ConfirmationModal : ComponentBase
{
    [Parameter]
    public bool IsVisible { get; set; }

    [Parameter]
    public string Message { get; set; } = "Weet u zeker dat u deze actie wilt uitvoeren?";

    [Parameter]
    public string ConfirmText { get; set; } = "Bevestigen";

    [Parameter]
    public string ConfirmButtonClass { get; set; } = "btn-danger";

    [Parameter]
    public bool ShowWarning { get; set; } = true;
    
    [Parameter]
    public EventCallback<bool> OnResult { get; set; }
}