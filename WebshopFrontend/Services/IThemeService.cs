namespace WebshopFrontend.Services;

public interface IThemeService
{
    event Action? OnThemeChanged; 
    
    string CurrentTheme { get; }
    
    Task InitializeThemeAsync();
    
    Task ToggleThemeAsync();
}