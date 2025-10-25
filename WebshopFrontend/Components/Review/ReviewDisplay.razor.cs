using Microsoft.AspNetCore.Components;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Review;

public partial class ReviewDisplay : ComponentBase
{
    [Parameter]
    public ReviewResponse Review { get; set; } = default!;

    [Parameter]
    public string? CurrentUserId { get; set; }

    [Parameter]
    public bool IsAdmin { get; set; }
    
    [Parameter]
    public EventCallback<int> OnEdit { get; set; }

    [Parameter]
    public EventCallback<int> OnDelete { get; set; }

    private bool IsAuthor => CurrentUserId != null && Review.UserId == CurrentUserId;
    private bool CanModify => IsAdmin || IsAuthor; 
    
    protected override void OnParametersSet()
    {
        if (Review == null)
        {
            throw new ArgumentNullException(nameof(Review), "Review parameter mag niet null zijn.");
        }
    }
}