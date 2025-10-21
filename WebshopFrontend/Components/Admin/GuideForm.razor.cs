using Microsoft.AspNetCore.Components;
using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Components.Admin;

public partial class GuideForm : ComponentBase
{
    [Parameter]
    [EditorRequired]
    public GuideRequest GuideModel { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public EventCallback OnValidSubmit { get; set; }

    [Parameter]
    public string ButtonText { get; set; } = "Opslaan";

    [Parameter]
    public bool IsSaving { get; set; }

    [Parameter]
    public string? ErrorMessage { get; set; }
}