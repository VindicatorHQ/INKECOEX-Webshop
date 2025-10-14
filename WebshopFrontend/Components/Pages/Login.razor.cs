using Microsoft.AspNetCore.Components;
using WebshopFrontend.Agents;
using WebshopFrontend.DTOs.Requests;

namespace WebshopFrontend.Components.Pages;

public partial class Login(IAuthAgent authAgent) : ComponentBase
{
    private LoginRequest LoginModel = new();
    private bool ShowLoginFailed;

    private async Task HandleLogin()
    {
        ShowLoginFailed = false;

        var success = await authAgent.LoginAsync(LoginModel);

        if (success)
        {
            NavManager.NavigateTo("/", true);
        }
        else
        {
            ShowLoginFailed = true;
        }
    }
}