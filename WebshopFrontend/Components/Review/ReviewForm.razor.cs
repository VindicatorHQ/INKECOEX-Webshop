using Microsoft.AspNetCore.Components;
using WebshopFrontend.DTOs.Requests;
using WebshopFrontend.DTOs.Responses;

namespace WebshopFrontend.Components.Review;

public partial class ReviewForm : ComponentBase
{
    [Parameter]
    public ReviewResponse? ReviewToEdit { get; set; }

    [Parameter]
    public EventCallback<ReviewRequest> OnSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private ReviewRequest Request = new();

    protected override void OnInitialized()
    {
        if (ReviewToEdit != null)
        {
            Request = new ReviewRequest
            {
                Comment = ReviewToEdit.Comment,
                Rating = ReviewToEdit.Rating,
                ReviewerName = ReviewToEdit.ReviewerName
            };
        }
    }

    private void SetRating(int rating)
    {
        Request.Rating = rating;

        StateHasChanged();
    }

    private async Task HandleValidSubmit()
    {
        await OnSubmit.InvokeAsync(Request);
    }
}