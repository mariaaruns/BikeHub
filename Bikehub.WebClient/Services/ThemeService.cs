namespace BikeHub.Services;

public class ThemeService
{
    public bool IsDark { get; private set; } = true;
    public string ThemeClass => IsDark ? "theme-dark" : "theme-light";
    public string ToggleIcon  => IsDark ? "☀" : "☾";
    public event Action? OnChanged;
    public void Toggle() { IsDark = !IsDark; OnChanged?.Invoke(); }
}
